namespace scienide.Common.Game;

using SadConsole;
using SadRogue.Primitives;

public class GlyphBeautifier
{
    public static readonly Dictionary<char, ColoredGlyphAndEffect> GlyphAppearanceMap = new()
    {
        { '#', new ColoredGlyphAndEffect() { Foreground = Color.Gray, GlyphCharacter = '#' } },
        { '@', new ColoredGlyphAndEffect() { Foreground = Color.GreenYellow, GlyphCharacter = '@' } },
        { 'o', new ColoredGlyphAndEffect() { Foreground = Color.SandyBrown, GlyphCharacter = 'o' } },
        { '.', new ColoredGlyphAndEffect() { Foreground = Color.SlateGray, GlyphCharacter = '.' } }
    };
}
