namespace scienide.Runner;

using SadConsole;
using SadConsole.Quick;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game;
using scienide.Engine.Game.Actors;
using scienide.Engine.Game.Actors.Builder;
using scienide.Engine.Infrastructure;
using Keyboard = SadConsole.Input.Keyboard;

internal class GameEngine : ScreenObject
{
    private bool _awaitInput = false;
    private GameMap _gameMap;
    private TimeManager _timeManager;
    private Hero _hero;

    public GameEngine()
    {
        _gameMap = new GameMap(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);
        
        Children.Add(_gameMap.Surface);

        _timeManager = new TimeManager();
        _hero = SpawnHero();
        SpawnMonster();
    }

    public override void Update(TimeSpan delta)
    {
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
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GameObjType.ActorPlayerControl);
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
        var spawnPoint = _gameMap.GetRandomSpawnPoint(GameObjType.ActorNonPlayerControl);
        var monster = new MonsterBuilder(spawnPoint)
            .SetGlyph('o')
            .SetTimeEntity(new ActorTimeEntity(-200, 50))
            .SetName("Snail")
            .Build();
        SpawnActor(monster);
    }

    private void SpawnActor(IActor actor)
    {
        _gameMap[actor.Position].AddChild(actor);
        _gameMap.Surface.SetGlyph(actor.Position.X, actor.Position.Y, _gameMap[actor.Position].Glyph.Char);
        _timeManager.Add(actor.TimeEntity ?? throw new ArgumentNullException(nameof(TimeEntity)));
    }
}
