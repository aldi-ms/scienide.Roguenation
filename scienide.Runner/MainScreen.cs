namespace scienide.Runner;

using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using scienide.Common.Infrastructure;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using scienide.Engine.Game;
using scienide.Engine.Game.Actors.Builder;
using scienide.UI;

internal class MainScreen : GameScreenBase
{
    private readonly ScreenSurface _sidePanelSurface;
    private readonly ScreenSurface _logPanel;

    public MainScreen(int width, int height, Point position)
        : base(width, height, position, MapGenerationStrategy.WaveFunctionCollapse,
            "../../../../../scienide.WaveFunctionCollapse/inputs/sample1.in")
    {
        _logPanel = new ScreenSurface(GameConfiguration.LogPanelSize.X, GameConfiguration.LogPanelSize.Y)
        {
            Position = new Point(1, GameConfiguration.PlayScreenSize.Y + GameConfiguration.BorderSize.Y + 1),
            UseKeyboard = true,
            UseMouse = false,
            IsFocused = false
        };

        _sidePanelSurface = new ScreenSurface(GameConfiguration.SidePanelSize.X, GameConfiguration.SidePanelSize.Y)
        {
            Position = GameConfiguration.SideBarIsOnRight ? new Point(1, 1) : new Point(GameConfiguration.PlayScreenSize.X + GameConfiguration.BorderSize.X + 1, 1),
            UseKeyboard = true,
            UseMouse = false,
            IsFocused = false
        };

        Border.CreateForSurface(Map.Surface, "Map");
        Border.CreateForSurface(_logPanel, "Game log");
        Border.CreateForSurface(_sidePanelSurface, "Info");

        for (int i = 0; i < 20; i++)
            SpawnMonster(i);

        _ = new GameLogPanel(_logPanel.Surface, _logPanel.Height - 1, Hero);
        _ = new SideInfoPanel(_sidePanelSurface.Surface);

        Children.Add(_logPanel);
        Children.Add(_sidePanelSurface);
        Children.Add(Map.Surface);
    }

    public override bool HandleMouseState(IScreenObject screenObject, SadConsole.Input.MouseScreenObjectState state)
    {
        if (state.Mouse.LeftClicked)
        {
            var selectedCell = Map[state.CellPosition];

            if (selectedCell.Properties[Props.IsVisible])
            {
                MessageBroker.Instance.Broadcast(new SelectedCellChangedArgs(selectedCell));
            }
            else if (selectedCell.Properties[Props.HasBeenSeen])
            {
                var cellData = SeenCells[selectedCell.Position];
                MessageBroker.Instance.Broadcast(new SelectedCellChangedArgs(cellData));
            }

            return true;
        }

        return false;
    }
}
