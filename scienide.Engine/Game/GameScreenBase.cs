﻿namespace scienide.Engine.Game;

using Microsoft.Extensions.Configuration;
using SadConsole;
using SadConsole.Input;
using SadConsole.Quick;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Common.Logging;
using scienide.Engine.Components;
using scienide.Engine.Game.Actors;
using scienide.Engine.Game.Actors.Builder;
using scienide.Engine.Map;
using scienide.WaveFunctionCollapse;
using Serilog;
using System;
using System.Diagnostics;

public abstract class GameScreenBase : ScreenObject
{
    private const int UpdateTimeBeforeWarningsMs = 150;

    private readonly GameMap _gameMap;
    private readonly TimeManager _timeManager;
    private readonly HashSet<Cell> _resetVisibilityCells;
    private readonly ILogger _logger;

    private bool _awaitInput = false;
    private Hero _hero;

    public GameScreenBase(int width, int height, Point position, MapGenerationStrategy mapStrategy, string wfcInputFile)
    {
        var logConfig = new LoggerConfiguration()
            .WriteTo.File($"Logs\\GameEngine-{DateTime.Today:yy-MM-dd}.log")
            .WriteTo.Debug()
            .MinimumLevel.Debug();
        _logger = Logging.ConfigureNamedLogger($"{nameof(Engine)}.{nameof(GameScreenBase)}", logConfig);

        _logger.Information("=== Starting Game ===");

        var mapTimer = Stopwatch.StartNew();

        _timeManager = [];
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

        _gameMap = new GameMap(gameMapSurface, map, !EnableFov);
        Children.Add(_gameMap.Surface);

        mapTimer.Stop();
        _logger.Information($"[{mapStrategy}] map generation took: {mapTimer.ElapsedTicks} ticks, {mapTimer.ElapsedMilliseconds}ms.");

        mapTimer.Restart();

        var regions = FloodFillGeneration.FloodFillMap(_gameMap);
        FloodFillGeneration.ConnectMapRegions(regions);
        //Common.Map.MapUtils.ColorizeRegions(_gameMap, regions);

        mapTimer.Stop();
        _logger.Information($"[{mapStrategy}] map flood fill took: {mapTimer.ElapsedTicks} ticks, {mapTimer.ElapsedMilliseconds}ms.");

        _hero = SpawnHero();

        if (EnableFov)
        {
            _gameMap.FoV.Compute(_hero.Position, _hero.FoVRange);
        }
    }

    public ILogger EngineLogger => _logger;

    public static bool EnableFov => Global.EnableFov;

    public GameMap Map => _gameMap;

    public Hero Hero => _hero;

    public Dictionary<Point, Cell> SeenCells { get; } = [];

    public abstract bool HandleMouseState(IScreenObject screenObject, MouseScreenObjectState state);

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        for (int i = 0; i < _timeManager.ActorCount; i++)
        {
            if (!_awaitInput)
            {
                _timeManager.ProgressSentinel();
            }

            _awaitInput = _timeManager.ProgressTime();

            if (EnableFov && Map.DirtyCells.Count > 0)
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
        }
    }

    public override void Render(TimeSpan delta)
    {
        base.Render(delta);

        foreach (var cell in _gameMap.DirtyCells)
        {
            if (EnableFov)
            {
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
            }
            else
            {
                _gameMap.Surface.SetCellAppearance(cell.Position.X, cell.Position.Y, cell.Glyph.Appearance);
            }
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

    public Hero SpawnHero()
    {
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GObjType.Player);
        _hero = (Hero)new HeroBuilder(spawnPoint)
            .SetGlyph('@')
            .SetFoVRange(10)
            .SetName("SCiENiDE")
            .SetTimeEntity(new ActorTimeEntity(-100, 100))
            .SetCombatComponent()
            .Build();

        SpawnActor(_hero);

        var inputController = new InputController(_hero);
        _gameMap.Surface.WithKeyboard(inputController.HandleKeyboard);

        EngineLogger.Information($"{nameof(SpawnHero)} spawned {_hero.Name}:{_hero.TypeId}.");

        return _hero;
    }

    public void SpawnMonster(int n)
    {
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GObjType.NPC);
        var monster = new MonsterBuilder(spawnPoint, "Snail " + n)
            .SetGlyph('o')
            .SetFoVRange(10)
            .SetTimeEntity(new ActorTimeEntity(-100, 50))
            .SetCombatComponent()
            .Build();
        SpawnActor(monster);

        EngineLogger.Information($"{nameof(SpawnMonster)} spawned {monster.Name}:{monster.TypeId}.");
    }

    // TODO: This should probably be moved somewhere else
    private void SpawnActor(Actor actor)
    {
        _gameMap[actor.Position].AddComponent(actor);
        if (!EnableFov)
        {
            _gameMap.Surface.SetCellAppearance(actor.Position.X, actor.Position.Y, actor.Glyph.Appearance);
        }

        _timeManager.Add(actor.TimeEntity ?? throw new ArgumentNullException(nameof(actor)));

        actor.SubscribeForMessages();

        _logger.Information($"Spawned actor {actor.Name}:{actor.TypeId}.");
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
}
