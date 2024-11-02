namespace scienide.Common;

using SadConsole;
using scienide.Common.Game;

public static class Extensions
{
    public static void SetGlyphWithAppearance(this ScreenSurface screenObject, int x, int y, char ch)
    {
        if (GlyphBeautifier.GlyphAppearanceMap.TryGetValue(ch, out var glyphAppearance))
        {
            screenObject.SetCellAppearance(x, y, glyphAppearance);
        }
        else
        {
            screenObject.SetGlyph(x, y, ch);
        }
    }
}
