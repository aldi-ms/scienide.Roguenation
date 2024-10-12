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
        _hero = InitHeroActor();
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
    }

    private Hero InitHeroActor()
    {
        var heroSpawn = _gameMap.GetRandomSpawnPoint(GameObjType.ActorPlayerControl);
        var hero = HeroBuilder.CreateBuilder(heroSpawn)
            .AddGlyph('@')
            .SetTimeEntity(100, 100, 50)
            .Build();
        _gameMap.Data[heroSpawn].AddChild(hero);
        _gameMap.Surface.SetGlyph(heroSpawn.X, heroSpawn.Y, _gameMap.Data[heroSpawn].Glyph.Char);

        var inputController = new InputController(hero);
        _gameMap.Surface.WithKeyboard(inputController.HandleKeyboard);
        _timeManager.Add(hero.TimeEntity ?? throw new ArgumentNullException());

        return hero;
    }
}
