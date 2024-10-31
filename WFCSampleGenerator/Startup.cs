namespace WFCSampleGenerator;

using SadConsole;
using SadConsole.Configuration;

internal class Startup
{
    static void Main()
    {
        Builder configuration = new Builder()
            .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
            .SetStartingScreen<RootScreen>()
            .ConfigureFonts(true)
            .IsStartingScreenFocused(false);

        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
}
