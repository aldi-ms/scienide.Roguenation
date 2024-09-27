using SadConsole;
using scienide.Engine.Game;

namespace scienide.Runner;

internal class PlayScreen : ScreenObject
{
    private GameMap _gameMap;

    public PlayScreen()
    {
        _gameMap = new GameMap(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);

        Children.Add(_gameMap.Surface);
    }
}
