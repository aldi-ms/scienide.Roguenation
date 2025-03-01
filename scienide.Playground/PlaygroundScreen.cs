namespace scienide.Playground;

using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Common.Messaging;
using scienide.Engine.Game;
using scienide.Engine.Game.Actors;
using scienide.UI;
using System.Diagnostics;

internal class PlaygroundScreen : GameScreenBase
{
    private readonly ScreenSurface _infoPanelSurface;
    private readonly ScreenSurface _consolePanel;

    public PlaygroundScreen(int width, int height)
        : base(width, height, new Point(1, 1), MapGenerationStrategy.WaveFunctionCollapse, @"../../../../../scienide.WaveFunctionCollapse/inputs/sample1.in")
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

        for (int i = 0; i < 1; i++)
            SpawnMonster(i, new Common.Game.Components.ActorCombatStats() { Attack = 1, Defense = 0, MaxHealth = 4 });

        _ = new GameLogPanel(_consolePanel.Surface, _consolePanel.Height - 1, new Hero(Point.Zero));
        _ = new SideInfoPanel(_infoPanelSurface.Surface, Hero);

#if ENABLE_FOV
        //Map.FoV.Compute(Hero.Position, Hero.FoVRange);
#endif

        Children.Add(_consolePanel);
        Children.Add(_infoPanelSurface);
        Children.Add(Map.Surface);
    }

    public override bool HandleMouseState(IScreenObject screenObject, MouseScreenObjectState state)
    {
        if (state.Mouse.RightClicked)
        {
            var selectedCell = Map[state.CellPosition];
            switch (selectedCell.Glyph.Char)
            {
                case ' ':
                case '.':
                    selectedCell.Terrain = new Terrain(new Glyph('#'));
                    break;
                case '#':
                    selectedCell.Terrain = new Terrain(new Glyph('.'));
                    break;
                default:
                    Trace.WriteLine($"Unknown option, glyph was: {selectedCell.Glyph}.");
                    break;
            }

            Map.DirtyCells.Add(selectedCell);
            return true;
        }

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
                MessageBroker.Instance.Broadcast(new SelectedCellChanged(selectedCell), true);
            }
            else if (selectedCell.Properties[Props.HasBeenSeen])
            {
                var cellData = SeenCells[selectedCell.Position];
                MessageBroker.Instance.Broadcast(new SelectedCellChanged(cellData), true);
            }

            return true;
        }

        return false;
    }
}

public static class GameConfig
{
    public const bool SideBarIsOnRight = false;

    public static readonly Point BorderSize = new(2, 2);

    public static readonly Point PlayScreenSize = new(24, 12);
    public static readonly Point LogPanelSize = new(FullScreenSize.X, 7);
    public static readonly Point SidePanelSize = new(40, FullScreenSize.Y);

    public static readonly Point FullScreenSize = PlayScreenSize + LogPanelSize + SidePanelSize + BorderSize;
}
