namespace scienide.Engine.Game;

public struct Glyph
{
    // TODO: Implement back/foreground colors, "animated" glyph / rotating characters
    
    public char Char { get; set; }

    public Glyph(char ch)
    {
        Char = ch;
    }
}
