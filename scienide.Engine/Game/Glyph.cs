using scienide.Engine.Core;
using SadRogue.Primitives;
namespace scienide.Engine.Game;

public class Glyph : GameComponent
{
    public char Char { get; set; }

    public Glyph(char ch, Point pos) : base(pos)
    {
        Char = ch;
    }
}
