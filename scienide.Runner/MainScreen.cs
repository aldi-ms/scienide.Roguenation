namespace scienide.Runner;

using SadConsole;
using SadConsole.Quick;
using SadConsole.UI;
using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using scienide.Engine.Game;
using scienide.Engine.Game.Actors;
using scienide.Engine.Game.Actors.Builder;
using scienide.UI;
using scienide.WaveFunctionCollapse;
using Keyboard = SadConsole.Input.Keyboard;

internal class MainScreen : ScreenObject
{
    private const bool SideBarIsRightHandSide = false;

    //private readonly Stopwatch _perfWatch = new();

    private readonly GameMap _gameMap;
    private readonly ScreenSurface _infoPanelSurface;
    private readonly ScreenSurface _consolePanel;
    private readonly TimeManager _timeManager;
    private readonly Hero _hero;

    private bool _awaitInput = false;

    public MainScreen()
    {
        var gameMapSurface = new ScreenSurface(GameSettings.PlayScreenSize.X - GameSettings.BorderSize.X, GameSettings.PlayScreenSize.Y - (GameSettings.BorderSize.Y * 2) + 1)
        {
            Position = SideBarIsRightHandSide ? new Point(GameSettings.SidePanelSize.X + GameSettings.BorderSize.X, 1) : new Point(1, 1),
            UseKeyboard = true,
            UseMouse = true,
            IsFocused = true
        };
        Border.CreateForSurface(gameMapSurface, "Map");
        gameMapSurface.WithMouse(HandleMouseState);

        _consolePanel = new ScreenSurface(GameSettings.FullScreenSize.X - GameSettings.BorderSize.X, GameSettings.LogPanelSize.Y + 1)
        {
            Position = new Point(1, gameMapSurface.Height + (GameSettings.BorderSize.Y * 2)),
            UseKeyboard = true,
            UseMouse = false,
            IsFocused = false
        };
        Border.CreateForSurface(_consolePanel, "Console");

        _infoPanelSurface = new ScreenSurface(GameSettings.SidePanelSize.X - GameSettings.BorderSize.X, GameSettings.FullScreenSize.Y - _consolePanel.Height - GameSettings.BorderSize.Y * 2)
        {
            Position = SideBarIsRightHandSide ? new Point(1, 1) : new Point(GameSettings.PlayScreenSize.X + GameSettings.BorderSize.X, 1),
            UseKeyboard = true,
            UseMouse = false,
            IsFocused = false
        };
        Border.CreateForSurface(_infoPanelSurface, "Info");

        var waveGenerator = new WaveGenerator(
            gameMapSurface.Width,
            gameMapSurface.Height,
            3,
            "../../../../../scienide.WaveFunctionCollapse/inputs/sample1.in");
        var mapArray = waveGenerator.Run()
            ?? throw new ArgumentNullException(nameof(WaveGenerator.Run));
        _gameMap = new GameMap(gameMapSurface, mapArray);

        _timeManager = new TimeManager();
        _hero = SpawnHero();

        var log = new GameLogPanel(_consolePanel.Surface, _consolePanel.Height - 1, _hero);
        var infoPanel = new InfoPanel(_infoPanelSurface.Surface);

        SpawnMonster();

        Children.Add(_consolePanel);
        Children.Add(_infoPanelSurface);
        Children.Add(_gameMap.Surface);
    }

    private bool HandleMouseState(IScreenObject screenObject, SadConsole.Input.MouseScreenObjectState state)
    {
        if (state.Mouse.LeftClicked)
        {
            // TODO: issue here with actor = null?

            var selectedCell = _gameMap[state.CellPosition];
            MessageBroker.Instance.Broadcast(new SelectedCellChangedEventArgs(selectedCell));
            return true;
        }

        return false;
    }

    public override void Update(TimeSpan delta)
    {
        //_perfWatch.Restart();

        base.Update(delta);

        if (!_awaitInput)
        {
            _timeManager.ProgressSentinel();
        }

        _awaitInput = _timeManager.ProgressTime();

        foreach (var cell in _gameMap.DirtyCells)
        {
            _gameMap.Surface.SetGlyph(cell.Position.X, cell.Position.Y, cell.Glyph.Char);
        }

        _gameMap.Surface.IsDirty = true;
        _gameMap.DirtyCells.Clear();
        //_perfWatch.Stop();

        /// TODO: Push data to db to gain performance data,
        //Trace.WriteLine($"{nameof(GameEngine)}.{nameof(Update)} took {_perfWatch.ElapsedTicks / 1000f} ticks to execute.");
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (!_gameMap.Surface.ProcessKeyboard(keyboard))
        {
            return base.ProcessKeyboard(keyboard);
        }

        return true;
    }

    private Hero SpawnHero()
    {
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GObjType.ActorPlayerControl);
        var hero = new HeroBuilder(spawnPoint)
            .SetGlyph('@')
            .SetName("SCiENiDE")
            .SetTimeEntity(new ActorTimeEntity(-100, 100))
            .Build();

        SpawnActor(hero);

        var inputController = new InputController(hero);
        _gameMap.Surface.WithKeyboard(inputController.HandleKeyboard);

        return (Hero)hero;
    }

    private void SpawnMonster()
    {
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GObjType.ActorNonPlayerControl);
        var monster = new MonsterBuilder(spawnPoint)
            .SetGlyph('o')
            .SetTimeEntity(new ActorTimeEntity(-100, 50))
            .SetName("Snail")
            .Build();
        SpawnActor(monster);
    }

    private void SpawnActor(Actor actor)
    {
        _gameMap[actor.Position].AddChild(actor);
        _gameMap.Surface.SetGlyph(actor.Position.X, actor.Position.Y, _gameMap[actor.Position].Glyph.Char);
        _timeManager.Add(actor.TimeEntity ?? throw new ArgumentNullException(nameof(actor)));

        MessageBroker.Instance.Subscribe<GameMessageEventArgs>(actor.Listener, actor);
    }
}
