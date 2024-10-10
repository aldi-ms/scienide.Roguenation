using SadConsole;
using SadConsole.Input;
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

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (!_gameMap.Surface.ProcessKeyboard(keyboard))
        {
            return base.ProcessKeyboard(keyboard);
        }

        return true;
    }
}
