namespace scienide.Runner;

using SadConsole;
using SadConsole.Quick;
using scienide.Engine.Core;
using scienide.Engine.Game;
using scienide.Engine.Game.Actors;
using scienide.Engine.Infrastructure;
using Keyboard = SadConsole.Input.Keyboard;

internal class GameEngine : ScreenObject
{
    private GameMap _gameMap;
    private TimeManager _timeManager;
    private Hero _hero;

    public GameEngine()
    {
        _gameMap = new GameMap(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);
        _timeManager = new TimeManager();
        _hero = SpawnHeroActor();
        Children.Add(_gameMap.Surface);
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (!_gameMap.Surface.ProcessKeyboard(keyboard))
        {
            return base.ProcessKeyboard(keyboard);
        }

        return true;
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        _timeManager.ProgressSentinel();
        _timeManager.ProgressTime();

        foreach (var cell in _gameMap.DirtyCells)
        {
            _gameMap.Surface.SetGlyph(cell.Position.X, cell.Position.Y, cell.Glyph.Char);
        }

        _gameMap.Surface.IsDirty = true;
        _gameMap.DirtyCells.Clear();
    }

    private Hero SpawnHeroActor()
    {
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GameObjType.ActorPlayerControl);
        var hero = HeroBuilder.CreateBuilder(spawnPoint)
            .AddGlyph('@')
            .SetHeroTimeEntity(-100, 200, 50)
            .Build();
        _gameMap.Data[spawnPoint].AddChild(hero);
        _gameMap.Surface.SetGlyph(spawnPoint.X, spawnPoint.Y, _gameMap.Data[spawnPoint].Glyph.Char);

        var inputController = new InputController(hero);
        _gameMap.Surface.WithKeyboard(inputController.HandleKeyboard);
        _timeManager.Add(hero.TimeEntity ?? throw new ArgumentNullException());

        return hero;
    }
}
