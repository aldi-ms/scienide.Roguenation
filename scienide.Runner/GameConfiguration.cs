namespace scienide.Runner;

using SadRogue.Primitives;
using scienide.Common;
using System.Runtime.InteropServices;

internal static partial class User32
{
    [LibraryImport("user32.dll", EntryPoint = "GetSystemMetrics")]
    internal static partial int GetSystemMetrics(int nIndex);
}

public class GameConfiguration
{
    public const bool RunFullScreen = false;
    public const bool SideBarIsOnRight = false;

    private const int SM_CXSCREEN = 0; // Width
    private const int SM_CYSCREEN = 1; // Height

    private static Point _resSize = Point.None;
    private static Point _logPanelSize = Point.None;
    private static Point _sidePanelSize = Point.None;
    private static Point _playScreenSize = Point.None;

    public static readonly Point BorderSize = new(3, 3);

    public static Point PlayScreenSize
    {
        get
        {
            if (_playScreenSize != Point.None)
            {
                return _playScreenSize;
            }
            
            var width = (FullScreenSize.X / 4) * 3 - BorderSize.X;
            var height = (FullScreenSize.Y / 6) * 5 - BorderSize.Y;

            // Round the number to regionSize to fix play screen sizing issues
            if (width % Global.MapGenRegionSize != 0)
            {
                width -= width % Global.MapGenRegionSize;
            }
            if (height % Global.MapGenRegionSize != 0)
            {
                height -= height % Global.MapGenRegionSize;
            }

            _playScreenSize = new Point(width, height);
            return _playScreenSize;
        }
    }

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

            var screenResolutionWidth = 1280;
            var screenResolutionHeight = 720;

            if (GameConfiguration.RunFullScreen)
            {
#pragma warning disable CS0162 // Unreachable code detected
                screenResolutionWidth = User32.GetSystemMetrics(SM_CXSCREEN);
#pragma warning restore CS0162 // Unreachable code detected
                screenResolutionHeight = User32.GetSystemMetrics(SM_CYSCREEN);
            }

            var width = screenResolutionWidth / 16;
            var height = screenResolutionHeight / 16;

            _resSize = new Point(width, height);
            return _resSize;
        }
    }
}
