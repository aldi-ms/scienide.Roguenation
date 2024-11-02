namespace scienide.Common.Game;

using SadConsole;

public readonly struct Glyph(ColoredGlyphAndEffect glyph)
{
    public Glyph(char ch) 
        : this(new ColoredGlyphAndEffect() { GlyphCharacter = ch })
    {
    }

    public ColoredGlyphAndEffect Appearance { get; } = glyph;

    public char Char => Appearance.GlyphCharacter;

    public override string ToString() => Appearance.GlyphCharacter.ToString();
}
