namespace WFCSampleGenerator;

using SadRogue.Primitives;

internal static class GameSettings
{
    public const int GAME_WIDTH = 120;
    public const int GAME_HEIGHT = 40;

    public const int CONTROLS_WIDTH = 36;
    public const int CONTROLS_HEIGHT = 10;

    public static readonly Rectangle ControlsScreenRect = new(2, 2, CONTROLS_WIDTH, CONTROLS_HEIGHT);

    public static int DrawScreenWidth = 9;
    public static int DrawScreenHeight = 9;
}
