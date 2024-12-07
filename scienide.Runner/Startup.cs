﻿namespace scienide.Runner;

using SadConsole;
using SadConsole.Configuration;
using SadRogue.Primitives;

internal class Startup
{
    internal static void Main()
    {
        Settings.WindowTitle = "SCiENiDE.ROGUENATiON";
        Builder configuration = new Builder()
            .SetScreenSize(GameConfiguration.FullScreenSize.X, GameConfiguration.FullScreenSize.Y)
            .SetStartingScreen(_ =>
            {
                return new MainScreen(
                    GameConfiguration.PlayScreenSize.X,
                    GameConfiguration.PlayScreenSize.Y,
                    GameConfiguration.SideBarIsOnRight ? new Point(GameConfiguration.SidePanelSize.X + GameConfiguration.BorderSize.X + 1, 1) : new Point(1, 1));
            })
            .ConfigureFonts(".\\Fonts\\C64.font")
            .IsStartingScreenFocused(true);

        Game.Create(configuration);
        Game.Instance.Started += Instance_Started;
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    private static void Instance_Started(object? sender, GameHost e)
    {
        if (sender is Game gameSender)
        {
            gameSender.ToggleFullScreen();
        }
    }
}
