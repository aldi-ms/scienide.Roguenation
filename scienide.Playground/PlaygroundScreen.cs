namespace scienide.Playground;

using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Engine.Game;
using scienide.Engine.Game.Actors;
using scienide.UI;
using System.Diagnostics;

internal class PlaygroundScreen : GameScreenBase
{
    private readonly ScreenSurface _infoPanelSurface;
    private readonly ScreenSurface _consolePanel;

    public PlaygroundScreen(int width, int height)
        : base(width, height, new Point(1, 1), MapGenerationStrategy.Empty, string.Empty)
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

        for (int i = 0; i < 50; i++)
            SpawnMonster(i);

        _ = new GameLogPanel(_consolePanel.Surface, _consolePanel.Height - 1, new Hero(Point.Zero));
        _ = new SideInfoPanel(_infoPanelSurface.Surface);

        Children.Add(_consolePanel);
        Children.Add(_infoPanelSurface);
        Children.Add(Map.Surface);
    }

    public override bool HandleMouseState(IScreenObject screenObject, MouseScreenObjectState state)
    {
        if (state.Mouse.LeftClicked)
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

        return false;
    }
}

public static class GameConfig
{
    public const bool SideBarIsOnRight = false;

    public static readonly Point BorderSize = new(2, 2);

    public static readonly Point PlayScreenSize = new(119, 42);
    public static readonly Point LogPanelSize = new(FullScreenSize.X, 7);
    public static readonly Point SidePanelSize = new(40, FullScreenSize.Y);

    public static readonly Point FullScreenSize = PlayScreenSize + LogPanelSize + SidePanelSize + BorderSize;
}
