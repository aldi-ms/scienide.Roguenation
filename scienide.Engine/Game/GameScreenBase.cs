namespace scienide.Engine.Game;

using SadConsole;
using SadConsole.Input;
using SadConsole.Quick;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Components;
using scienide.Common.Infrastructure;
using scienide.Common.Logging;
using scienide.Common.Messaging;
using scienide.Engine.Game.Actors;
using scienide.Engine.Game.Actors.Builder;
using scienide.Engine.Map;
using scienide.WaveFunctionCollapse;
using Serilog;
using System;
using System.Diagnostics;

public abstract class GameScreenBase : ScreenObject, IDisposable
{
    private const int UpdateTimeBeforeWarningsMs = 150;

    private readonly GameMap _gameMap;
    private readonly TurnManager _turnManager;
    private readonly HashSet<Cell> _resetVisibilityCells;
    private readonly ILogger _logger;

    private Hero _hero;
    private bool _disposedValue;

    public GameScreenBase(int width, int height, Point position, MapGenerationStrategy mapStrategy, string wfcInputFile)
    {
        var logConfig = new LoggerConfiguration()
            .WriteTo.File($"Logs\\GameEngine-{DateTime.Today:yy-MM-dd}.log")
            .WriteTo.Debug()
            .MinimumLevel.Debug();
        _logger = Logging.ConfigureNamedLogger($"{nameof(Engine)}.{nameof(GameScreenBase)}", logConfig);

        _logger.Information("=== Starting Game ===");

        var mapTimer = Stopwatch.StartNew();

        _turnManager = new TurnManager();
        _resetVisibilityCells = [];

        var map = mapStrategy switch
        {
            MapGenerationStrategy.Empty => CreateEmptyMap(width, height),
            MapGenerationStrategy.WaveFunctionCollapse => GenerateGameMap(width, height, wfcInputFile, Global.MapGenRegionSize),
            _ => throw new NotImplementedException(mapStrategy.ToString()),
        };

        var gameMapSurface = new ScreenSurface(map.Width, map.Height)
        {
            Position = position,
            UseKeyboard = true,
            UseMouse = true,
            IsFocused = true
        };
        gameMapSurface.WithMouse(HandleMouseState);

        _gameMap = new GameMap(gameMapSurface, map/*, !EnableFov*/);
        Children.Add(_gameMap.Surface);

        mapTimer.Stop();
        _logger.Information($"[{mapStrategy}] map generation took: {mapTimer.ElapsedTicks} ticks, {mapTimer.ElapsedMilliseconds}ms.");

        mapTimer.Restart();

        var regions = FloodFillGeneration.FloodFillMap(_gameMap);
        FloodFillGeneration.ConnectMapRegions(regions);
        //Common.Map.MapUtils.ColorizeRegions(_gameMap, regions);

        mapTimer.Stop();
        _logger.Information($"[{mapStrategy}] map flood fill took: {mapTimer.ElapsedTicks} ticks, {mapTimer.ElapsedMilliseconds}ms.");

        _hero = SpawnHero(new ActorCombatStats { MaxHealth = 10, Attack = 2, Defense = 0 });

#if ENABLE_FOV
        _gameMap.FoV.Compute(_hero.Position, _hero.FoVRange);
#endif

        MessageBroker.Instance.Subscribe<ActorDeathMessage>(OnActorDeath);
    }

    public ILogger EngineLogger => _logger;

    public GameMap Map => _gameMap;

    public Hero Hero => _hero;

    public Dictionary<Point, Cell> SeenCells { get; } = [];

    public abstract bool HandleMouseState(IScreenObject screenObject, MouseScreenObjectState state);

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        _turnManager.ProcessNext();

#if ENABLE_FOV
        if (Map.DirtyCells.Count > 0)
        {
            foreach (var cell in _resetVisibilityCells)
            {
                cell.Properties[Props.IsVisible] = false;
                cell.Properties[Props.HasBeenSeen] = true;
                _gameMap.DirtyCells.Add(cell);
            }
            _resetVisibilityCells.Clear();

            var visibleCells = _gameMap.FoV.Compute(_hero.Position, _hero.FoVRange);

            for (int j = 0; j < visibleCells.Count; j++)
            {
                visibleCells[j].Properties[Props.IsVisible] = true;
                Map.DirtyCells.Add(visibleCells[j]);
            }
        }
#endif
    }

    public override void Render(TimeSpan delta)
    {
        base.Render(delta);

        foreach (var cell in _gameMap.DirtyCells)
        {
#if ENABLE_FOV
            if (cell.Properties[Props.IsVisible])
            {
                _gameMap.Surface.SetCellAppearance(cell.Position.X, cell.Position.Y, cell.Glyph.Appearance);
                _resetVisibilityCells.Add(cell);

                var cloned = cell.Clone(true);
                cloned.Glyph.Appearance.Background = new Color(cloned.Glyph.Appearance.Background, 0.75f);
                cloned.Glyph.Appearance.Foreground = cloned.Glyph.Appearance.Foreground.LerpSteps(Color.Gray, 3)[1];
                SeenCells[cell.Position] = cloned;
            }
            else if (SeenCells.TryGetValue(cell.Position, out var seenCell))
            {
                _gameMap.Surface.SetCellAppearance(cell.Position.X, cell.Position.Y, seenCell.Glyph.Appearance);
            }
#else
            _gameMap.Surface.SetCellAppearance(cell.Position.X, cell.Position.Y, cell.Glyph.Appearance);
#endif
        }

        _gameMap.DirtyCells.Clear();
    }

    public override bool ProcessKeyboard(SadConsole.Input.Keyboard keyboard)
    {
        if (!_gameMap.Surface.ProcessKeyboard(keyboard))
        {
            return base.ProcessKeyboard(keyboard);
        }

        return true;
    }

    public void SpawnMonster(int id, ActorCombatStats stats)
    {
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GObjType.NPC);
        var monster = new MonsterBuilder(spawnPoint, "Snail " + id)
            .SetGlyph('o')
            .SetFoVRange(7)
            .SetTimeEntity(new TimeEntity(-100, 75))
            .SetCombatComponent(stats)
            .Build();
        SpawnActor(monster);

        EngineLogger.Information($"{nameof(SpawnMonster)} spawned {monster.Name}:{monster.Id}.");
    }

    private Hero SpawnHero(ActorCombatStats stats)
    {
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GObjType.Player);
        _hero = (Hero)new HeroBuilder(spawnPoint)
            .SetGlyph('@')
            .SetFoVRange(10)
            .SetName("SCiENiDE")
            .SetTimeEntity(new TimeEntity(-100, 100))
            .SetCombatComponent(stats)
            .Build();

        SpawnActor(_hero);

        var inputController = new InputController(_hero);
        _gameMap.Surface.WithKeyboard(inputController.HandleKeyboard);

        EngineLogger.Information($"{nameof(SpawnHero)} spawned {_hero.Name}:{_hero.Id}.");

        return _hero;
    }

    // TODO: This should probably be moved somewhere else
    private void SpawnActor(Actor actor)
    {
        _gameMap[actor.Position].AddComponent(actor);
#if !ENABLE_FOV
     _gameMap.Surface.SetCellAppearance(actor.Position.X, actor.Position.Y, actor.Glyph.Appearance);
#endif
        ArgumentNullException.ThrowIfNull(actor.TimeEntity);

        _turnManager.AddEntity(actor.TimeEntity);
        actor.SubscribeForMessages();

        _logger.Information($"Spawned actor {actor.Name}:{actor.Id}.");
    }

    private void OnActorDeath(ActorDeathMessage message)
    {
        ArgumentNullException.ThrowIfNull(message.Actor.TimeEntity);

        MessageBroker.Instance.Broadcast(new SystemMessage($"{message.Actor.Name} was killed!"), true);

        Map.DirtyCells.Add(message.Actor.CurrentCell);
        _turnManager.RemoveEntity(message.Actor.TimeEntity);
        Map[message.Actor.Position].RemoveComponent(message.Actor);
        message.Actor.Dispose();
    }

    private FlatArray<Glyph> GenerateGameMap(int width, int height, string inputFileMap, int regionSize)
    {
        if (string.IsNullOrWhiteSpace(inputFileMap))
        {
            throw new ArgumentNullException(nameof(inputFileMap));
        }

        if (!File.Exists(inputFileMap))
        {
            throw new FileNotFoundException(inputFileMap);
        }

        var waveGenerator = new WaveGenerator(width, height, regionSize);
        var mapArray = waveGenerator.Run(inputFileMap)
            ?? throw new ArgumentNullException(nameof(inputFileMap));

        if (mapArray.Width != width || mapArray.Height != height)
        {
            _logger.Information($"Map width or height was approximated to divisible by regionSize = {regionSize}.");
        }

        var glyphArray = mapArray.Select(ch =>
        {
            if (GlyphData.GlyphAppearanceMap.TryGetValue(ch, out var appearance))
            {
                return new Glyph((ColoredGlyphAndEffect)appearance.Clone());
            }
            return new Glyph(ch);
        }).ToArray();

        return new FlatArray<Glyph>(mapArray.Width, mapArray.Height, glyphArray);
    }

    private static FlatArray<Glyph> CreateEmptyMap(int width, int height)
    {
        var mapData = new FlatArray<Glyph>(width, height);
        var emptyGlyph = GlyphData.GlyphAppearanceMap['.'];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = new Glyph((ColoredGlyphAndEffect)emptyGlyph.Clone());
            }
        }

        return mapData;
    }

    public enum MapGenerationStrategy
    {
        Empty,
        WaveFunctionCollapse
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~GameScreenBase()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
