using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

namespace scienide.Engine.Game.Actors;

public class Hero : Actor
{
    public Hero(string name, Point pos, Glyph glyph)
        : base(name, pos, glyph)
    {
    }

    public override IActionCommand TakeTurn()
    {
        return new RestAction(this);
    }
}
