namespace scienide.Common.Game;

using SadConsole;

public readonly struct Glyph(ColoredGlyphAndEffect glyph) : IEquatable<char>, IEquatable<Glyph>
{
    public Glyph(char ch)
        : this(new ColoredGlyphAndEffect() { GlyphCharacter = ch })
    {
    }

    public ColoredGlyphAndEffect Appearance { get; } = glyph;

    public char Char => Appearance.GlyphCharacter;

    public bool Equals(char other)
    {
        return Char.Equals(other);
    }

    public bool Equals(Glyph other)
    {
        return Char.Equals(other);
    }

    public override bool Equals(object? obj)
    {
        return obj != null && obj is Glyph glyph && Equals(glyph);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Char, Appearance.Effect, Appearance.Background, Appearance.Foreground, Appearance.IsVisible, Appearance.Mirror, Appearance.Decorators);
    }

    public override string ToString() => Appearance.GlyphCharacter.ToString();

    public static bool operator ==(Glyph left, Glyph right) => left.Equals(right);
    public static bool operator !=(Glyph left, Glyph right) => !left.Equals(right);
    public static bool operator ==(Glyph left, char right) => left.Equals(right);
    public static bool operator !=(Glyph left, char right) => !left.Equals(right);
    public static bool operator ==(char left, Glyph right) => left.Equals(right);
    public static bool operator !=(char left, Glyph right) => !left.Equals(right);
}
