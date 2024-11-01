namespace scienide.Common.Game;

using SadConsole;

public readonly struct Glyph(char ch)
{
    public ColoredGlyphAndEffect Appearance { get; } = new ColoredGlyphAndEffect() { GlyphCharacter = ch };

    public char Char => Appearance.GlyphCharacter;

    public override string ToString() => Appearance.GlyphCharacter.ToString();
}
