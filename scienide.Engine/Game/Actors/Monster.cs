using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

namespace scienide.Engine.Game.Actors;

public class Monster(Point pos) : Actor(pos)
{
    public override IActionCommand TakeTurn()
    {
        return new WalkAction(this, Global.GetRandomValidDirection());
    }
}
