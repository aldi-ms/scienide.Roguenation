using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public struct Glyph
{
    public char Char { get; set; }
    public Point Position { get; set; }
    public IGameComponent? Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Glyph(char ch, Point pos)
    {
        Char = ch;
        Position = pos;
    }
}
