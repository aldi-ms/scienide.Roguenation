namespace scienide.Runner;

using SadRogue.Primitives;

public static class GameSettings
{
    public static readonly Point BorderSize = new(2, 2);

    public static readonly Point PlayScreenSize = new(120, 42);
    public static readonly Point InfoPanelSize = new(FullScreenSize.X, 7);
    public static readonly Point SidePanelSize = new(40, FullScreenSize.Y);

    public static readonly Point FullScreenSize = PlayScreenSize + InfoPanelSize + SidePanelSize + BorderSize;
}
