namespace scienide.Runner;

using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using scienide.Engine.Game;
using scienide.UI;

internal class MainScreen : GameScreenBase
{
    private readonly ScreenSurface _infoPanelSurface;
    private readonly ScreenSurface _consolePanel;

    public MainScreen(int width, int height, Point position)
        : base(width, height, position, MapGenerationStrategy.WaveFunctionCollapse,
            "../../../../../scienide.WaveFunctionCollapse/inputs/sample1.in")
    {
        _consolePanel = new ScreenSurface(GameConfig.FullScreenSize.X - GameConfig.BorderSize.X, GameConfig.LogPanelSize.Y + 1)
        {
            Position = new Point(1, Map.Surface.Height + (GameConfig.BorderSize.Y * 2)),
            UseKeyboard = true,
            UseMouse = false,
            IsFocused = false
        };
        _infoPanelSurface = new ScreenSurface(GameConfig.SidePanelSize.X - GameConfig.BorderSize.X, GameConfig.FullScreenSize.Y - _consolePanel.Height - GameConfig.BorderSize.Y * 2)
        {
            Position = GameConfig.SideBarIsOnRight ? new Point(1, 1) : new Point(GameConfig.PlayScreenSize.X + GameConfig.BorderSize.X, 1),
            UseKeyboard = true,
            UseMouse = false,
            IsFocused = false
        };

        Border.CreateForSurface(Map.Surface, "Map");
        Border.CreateForSurface(_consolePanel, "Game log");
        Border.CreateForSurface(_infoPanelSurface, "Info");

        for (int i = 0; i < 20; i++)
            SpawnMonster();

        _ = new GameLogPanel(_consolePanel.Surface, _consolePanel.Height - 1, Hero);
        _ = new InfoPanel(_infoPanelSurface.Surface);

        Children.Add(_consolePanel);
        Children.Add(_infoPanelSurface);
        Children.Add(Map.Surface);
    }

    public override bool HandleMouseState(IScreenObject screenObject, SadConsole.Input.MouseScreenObjectState state)
    {
        if (state.Mouse.LeftClicked)
        {
            var selectedCell = Map[state.CellPosition];
            MessageBroker.Instance.Broadcast(new SelectedCellChangedEventArgs(selectedCell));
            return true;
        }

        return false;
    }
}
