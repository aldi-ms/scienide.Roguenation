using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public struct Glyph
{
    public char Char { get; set; }
    public IGameComponent? Parent { get; set; }

    public Glyph(char ch)
    {
        Char = ch;
    }
}
