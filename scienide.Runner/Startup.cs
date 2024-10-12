namespace scienide.Runner;

using SadConsole;
using SadConsole.Configuration;

internal class Startup
{
    internal static void Main(string[] args)
    {
        Settings.WindowTitle = "SCiENiDE.ROGUENATiON";
        Builder configuration = new Builder()
            .SetScreenSize(120, 42)
            .SetStartingScreen<GameEngine>()
            .ConfigureFonts(true)
            .IsStartingScreenFocused(true);

        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
}
