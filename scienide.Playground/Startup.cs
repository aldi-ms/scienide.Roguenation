namespace scienide.Playground;

using SadConsole;
using SadConsole.Configuration;

internal class Startup
{
    static void Main()
    {
        Settings.WindowTitle = "SCiENiDE.PLAYGROUND";
        Builder configuration = new Builder()
            .SetScreenSize(GameConfig.FullScreenSize.X, GameConfig.FullScreenSize.Y)
            .SetStartingScreen(gameHost => new PlaygroundScreen(
                    GameConfig.PlayScreenSize.X - GameConfig.BorderSize.X,
                    GameConfig.PlayScreenSize.Y - (GameConfig.BorderSize.Y * 2) + 1))
            .ConfigureFonts(true)
            .IsStartingScreenFocused(true);

        Game.Create(configuration);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }
}
