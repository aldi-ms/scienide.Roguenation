namespace scienide.Common.Game;

public struct Glyph(char ch)
{
    // TODO: Implement back/foreground colors, "animated" glyph / rotating characters

    public char Char { get; set; } = ch;

    public override string ToString() => Char.ToString();
}
