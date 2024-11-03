namespace scienide.Runner;

using SadConsole;
using SadConsole.Configuration;
using SadRogue.Primitives;

internal class Startup
{
    internal static void Main()
    {
        Settings.WindowTitle = "SCiENiDE.ROGUENATiON";
        Builder configuration = new Builder()
            .SetScreenSize(GameConfig.FullScreenSize.X, GameConfig.FullScreenSize.Y)
            .SetStartingScreen(_ =>
            {
                return new MainScreen(
                    GameConfig.PlayScreenSize.X - GameConfig.BorderSize.X,
                    GameConfig.PlayScreenSize.Y - (GameConfig.BorderSize.Y * 2) + 1,
                    GameConfig.SideBarIsOnRight ? new Point(GameConfig.SidePanelSize.X + GameConfig.BorderSize.X, 1) : new Point(1, 1));
            })
            .ConfigureFonts(true)
            .IsStartingScreenFocused(true);

        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
}
