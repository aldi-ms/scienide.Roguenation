namespace scienide.Runner;

using SadConsole;
using SadConsole.Configuration;

internal class Startup
{
    internal static void Main()
    {
        Settings.WindowTitle = "SCiENiDE.ROGUENATiON";
        Builder configuration = new Builder()
            .SetScreenSize(GameSettings.FullScreenSize.X, GameSettings.FullScreenSize.Y)
            .SetStartingScreen<MainScreen>()
            .ConfigureFonts(true)
            .IsStartingScreenFocused(true);

        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
}
