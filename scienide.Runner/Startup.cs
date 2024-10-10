using SadConsole;
using SadConsole.Configuration;

namespace scienide.Runner;

internal class Startup
{
    static void Main(string[] args)
    {
        Settings.WindowTitle = "SCiENiDE.ROGUENATiON";
        Builder configuration = new Builder()
            .SetScreenSize(120, 42)
            .SetStartingScreen<PlayScreen>()
            .ConfigureFonts(true)
            .IsStartingScreenFocused(true);

        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
}
