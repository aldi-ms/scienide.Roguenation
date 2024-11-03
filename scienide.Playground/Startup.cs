namespace scienide.Playground;

using SadConsole;
using SadConsole.Configuration;

internal class Startup
{
    static void Main()
    {
        Settings.WindowTitle = "SCiENiDE.PLAYGROUND";
        Builder configuration = new Builder()
            .SetScreenSize(120, 40)
            .SetStartingScreen(gameHost => new PlaygroundScreen(gameHost.ScreenCellsX, gameHost.ScreenCellsY))
            .ConfigureFonts(true)
            .IsStartingScreenFocused(true);

        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
}
