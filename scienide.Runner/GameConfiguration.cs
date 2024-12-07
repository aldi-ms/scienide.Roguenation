namespace scienide.Runner;

using SadRogue.Primitives;
using System.Runtime.InteropServices;

internal static partial class User32
{
    [LibraryImport("user32.dll", EntryPoint = "GetSystemMetrics")]
    internal static partial int GetSystemMetrics(int nIndex);
}

public class GameConfiguration
{
    private const int SM_CXSCREEN = 0; // Width
    private const int SM_CYSCREEN = 1; // Height

    private static Point _resSize = Point.None;
    private static Point _logPanelSize = Point.None;
    private static Point _sidePanelSize = Point.None;

    public const bool SideBarIsOnRight = true;

    public static readonly Point BorderSize = new(3, 3);

    public static Point PlayScreenSize => new((FullScreenSize.X / 4) * 3 - BorderSize.X, (FullScreenSize.Y / 6) * 5 - BorderSize.Y);

    public static Point SidePanelSize
    {
        get
        {
            if (_sidePanelSize != Point.None)
            {
                return _sidePanelSize;
            }

            _sidePanelSize = new((FullScreenSize.X / 4) - BorderSize.X, PlayScreenSize.Y);
            return _sidePanelSize;
        }
    }

    public static Point LogPanelSize
    {
        get
        {
            if (_logPanelSize != Point.None)
            {
                return _logPanelSize;
            }

            var y = FullScreenSize.Y - PlayScreenSize.Y - (BorderSize.Y * 2);
            _logPanelSize = new(FullScreenSize.X - BorderSize.X, y);
            return _logPanelSize;
        }
    }

    public static Point FullScreenSize
    {
        get
        {
            if (_resSize != Point.None)
            {
                return _resSize;
            }

            var screenResolutionWidth = User32.GetSystemMetrics(SM_CXSCREEN);
            var screenResolutionHeight = User32.GetSystemMetrics(SM_CYSCREEN);

            _resSize = new Point(screenResolutionWidth, screenResolutionHeight) / 16;
            return _resSize;
        }
    }
}
