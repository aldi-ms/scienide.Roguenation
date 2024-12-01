namespace scienide.Common.Game;

using SadConsole;
using SadRogue.Primitives;

public static class GlyphData
{
    public static readonly Dictionary<char, ColoredGlyphAndEffect> GlyphAppearanceMap = new()
    {
        { '#', new ColoredGlyphAndEffect() { Foreground = Color.Gray, Background = Color.DarkGray, GlyphCharacter = '#' } },
        { '@', new ColoredGlyphAndEffect() { Foreground = Color.GreenYellow, Background = Color.Transparent, GlyphCharacter = '@' } },
        { 'o', new ColoredGlyphAndEffect() { Foreground = Color.SandyBrown, Background = Color.Transparent, GlyphCharacter = 'o' } },
        { '.', new ColoredGlyphAndEffect() { Foreground = Color.SlateGray, GlyphCharacter = '.' } }
    };
}
