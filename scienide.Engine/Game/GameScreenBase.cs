﻿namespace scienide.Engine.Game;

using SadConsole;
using SadConsole.Input;
using SadConsole.Quick;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using scienide.Engine.FieldOfView;
using scienide.Engine.Game.Actors;
using scienide.Engine.Game.Actors.Builder;
using scienide.WaveFunctionCollapse;
using System;
using System.Diagnostics;

public abstract class GameScreenBase : ScreenObject
{
    public enum MapGenerationStrategy
    {
        Empty,
        WaveFunctionCollapse
    }

    private const int UpdateTimeBeforeWarningsMs = 150;
    private readonly GameMap _gameMap;
    private readonly TimeManager _timeManager;
    private readonly Visibility _fov;
    private readonly Stopwatch _timer;
    private Hero _hero;

    private bool _awaitInput = false;

    public GameScreenBase(int width, int height, Point position, MapGenerationStrategy mapStrategy, string wfcInputFile)
    {
        _timer = new Stopwatch();
        _timeManager = new TimeManager();
        var gameMapSurface = new ScreenSurface(width, height)
        {
            Position = position,
            UseKeyboard = true,
            UseMouse = true,
            IsFocused = true
        };

        gameMapSurface.WithMouse(HandleMouseState);

        var map = mapStrategy switch
        {
            MapGenerationStrategy.Empty => CreateEmptyMap(width, height),
            MapGenerationStrategy.WaveFunctionCollapse => GenerateGameMap(gameMapSurface.Width, gameMapSurface.Height, wfcInputFile),
            _ => throw new NotImplementedException(),
        };

        _gameMap = new GameMap(gameMapSurface, map, !EnableFov);
        Children.Add(_gameMap.Surface);

        _hero = SpawnHero();

        if (EnableFov)
        {
            _fov = new MyVisibility(_gameMap);
            _fov.Compute(_hero.Position, _hero.FoVRange);
        }
        else
        {
            _fov = VisibilityEmpty.Instance;
        }
    }

    public static bool EnableFov => Global.EnableFov;

    public GameMap Map => _gameMap;

    public Hero Hero => _hero;

    public abstract bool HandleMouseState(IScreenObject screenObject, MouseScreenObjectState state);

    public override void Update(TimeSpan delta)
    {
        _timer.Restart();
        base.Update(delta);
        if (!_awaitInput)
        {
            _timeManager.ProgressSentinel();
        }

        _awaitInput = _timeManager.ProgressTime();

        if (EnableFov && Map.DirtyCells.Count > 0)
        {
            _fov.Compute(_hero.Position, _hero.FoVRange);
        }

        foreach (var cell in _gameMap.DirtyCells)
        {
            if (EnableFov)
            {
                if (cell.Properties.GetProperty(Props.IsVisible))
                {
                    if (cell.Glyph.Char != ' ')
                    {
                        _gameMap.Surface.SetCellAppearance(cell.Position.X, cell.Position.Y, cell.Glyph.Appearance);
                    }
                    else
                    {
                        _gameMap.Surface.SetGlyph(cell.Position.X, cell.Position.Y, ',');
                    }
                }
                else
                {
                    _gameMap.Surface.SetGlyph(cell.Position.X, cell.Position.Y, cell.Glyph.Char == '@' ? '@' : '.');
                }
            }
            else
            {
                _gameMap.Surface.SetCellAppearance(cell.Position.X, cell.Position.Y, cell.Glyph.Appearance);
            }
        }

        _gameMap.DirtyCells.Clear();
        _timer.Stop();

        if (_timer.ElapsedMilliseconds > UpdateTimeBeforeWarningsMs)
        {
            Trace.WriteLine($"Update elapsed more than {UpdateTimeBeforeWarningsMs}ms: {_timer.ElapsedTicks}; {_timer.ElapsedMilliseconds}ms.");
            Trace.WriteLine(Environment.StackTrace);
        }
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
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GObjType.ActorPlayerControl);
        _hero = (Hero)new HeroBuilder(spawnPoint)
            .SetGlyph('@')
            .SetName("SCiENiDE")
            .SetTimeEntity(new ActorTimeEntity(-100, 100))
            .Build();

        SpawnActor(_hero);

        var inputController = new InputController(_hero);
        _gameMap.Surface.WithKeyboard(inputController.HandleKeyboard);

        return _hero;
    }

    public void SpawnMonster()
    {
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GObjType.ActorNonPlayerControl);
        var monster = new MonsterBuilder(spawnPoint)
            .SetGlyph('o')
            .SetTimeEntity(new ActorTimeEntity(-100, 50))
            .SetName("Snail")
            .Build();
        SpawnActor(monster);
    }

    private FlatArray<Glyph> GenerateGameMap(int width, int height, string inputFileMap)
    {
        if (string.IsNullOrWhiteSpace(inputFileMap))
        {
            throw new ArgumentNullException(nameof(inputFileMap));
        }

        if (!File.Exists(inputFileMap))
        {
            throw new FileNotFoundException(inputFileMap);
        }

        var waveGenerator = new WaveGenerator(width, height, 3);
        var mapArray = waveGenerator.Run(inputFileMap)
            ?? throw new ArgumentNullException(nameof(WaveGenerator.Run));

        var glyphArray = mapArray.Select(ch =>
        {
            if (GlyphBeautifier.GlyphAppearanceMap.TryGetValue(ch, out var appearance))
            {
                return new Glyph(appearance);
            }
            return new Glyph(ch);
        }).ToArray();

        return new FlatArray<Glyph>(width, height, glyphArray);
    }

    private void SpawnActor(Actor actor)
    {
        _gameMap[actor.Position].AddChild(actor);
        if (!EnableFov)
            _gameMap.Surface.SetCellAppearance(actor.Position.X, actor.Position.Y, actor.Glyph.Appearance);
        _timeManager.Add(actor.TimeEntity ?? throw new ArgumentNullException(nameof(actor)));

        MessageBroker.Instance.Subscribe<GameMessageEventArgs>(actor.Listener, actor);
    }

    private static FlatArray<Glyph> CreateEmptyMap(int width, int height)
    {
        var mapData = new FlatArray<Glyph>(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = new Glyph(' ');
            }
        }

        return mapData;
    }
}
