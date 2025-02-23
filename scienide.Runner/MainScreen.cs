namespace scienide.Runner;

using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using scienide.Common.Infrastructure;
using scienide.Common.Messaging;
using scienide.Engine.Game;
using scienide.UI;

internal class MainScreen : GameScreenBase
{
    private readonly SideInfoPanel _sidePanel;
    private readonly GameLogPanel _logPanel;

    public MainScreen(int width, int height, Point position)
        : base(width, height, position, MapGenerationStrategy.WaveFunctionCollapse,
            "../../../../../scienide.WaveFunctionCollapse/inputs/sample1.in")
    {
        var logPanel = new ScreenSurface(GameConfiguration.LogPanelSize.X, GameConfiguration.LogPanelSize.Y)
        {
            Position = new Point(1, GameConfiguration.PlayScreenSize.Y + GameConfiguration.BorderSize.Y + 1),
            UseKeyboard = true,
            UseMouse = false,
            IsFocused = false
        };

        var sidePanelSurface = new ScreenSurface(GameConfiguration.SidePanelSize.X, GameConfiguration.SidePanelSize.Y)
        {
            Position = GameConfiguration.SideBarIsOnRight ? new Point(1, 1) : new Point(GameConfiguration.PlayScreenSize.X + GameConfiguration.BorderSize.X + 1, 1),
            UseKeyboard = true,
            UseMouse = false,
            IsFocused = false
        };

        Border.CreateForSurface(Map.Surface, "Map");
        Border.CreateForSurface(logPanel, "Game log");
        Border.CreateForSurface(sidePanelSurface, "Info");

        for (int i = 0; i < 10; i++)
        {
            SpawnMonster(i);
        }

        _logPanel = new GameLogPanel(logPanel.Surface, logPanel.Height - 1, Hero);
        _sidePanel = new SideInfoPanel(sidePanelSurface.Surface, Hero);

        Children.Add(logPanel);
        Children.Add(sidePanelSurface);
        Children.Add(Map.Surface);
    }

    public override bool HandleMouseState(IScreenObject screenObject, SadConsole.Input.MouseScreenObjectState state)
    {
        if (state.Mouse.LeftClicked)
        {
#if ENABLE_FOV
            bool foVDisabled = false;
#else
            bool foVDisabled = true;
#endif

            var selectedCell = Map[state.CellPosition];

            if (foVDisabled || selectedCell.Properties[Props.IsVisible])
            {
                MessageBroker.Instance.Broadcast(new SelectedCellChanged(selectedCell));
            }
            else if (selectedCell.Properties[Props.HasBeenSeen])
            {
                var cellData = SeenCells[selectedCell.Position];
                MessageBroker.Instance.Broadcast(new SelectedCellChanged(cellData));
            }

            return true;
        }

        return false;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _sidePanel.Dispose();
        _logPanel.Dispose();
    }
}
