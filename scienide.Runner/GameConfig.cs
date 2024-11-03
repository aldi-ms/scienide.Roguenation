namespace scienide.Runner;

using SadRogue.Primitives;

public static class GameConfig
{
    public const bool SideBarIsOnRight = false;

    public static readonly Point BorderSize = new(2, 2);

    public static readonly Point PlayScreenSize = new(119, 42);
    public static readonly Point LogPanelSize = new(FullScreenSize.X, 7);
    public static readonly Point SidePanelSize = new(40, FullScreenSize.Y);

    public static readonly Point FullScreenSize = PlayScreenSize + LogPanelSize + SidePanelSize + BorderSize;
}
