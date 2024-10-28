namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

public class Monster(Point pos) : Actor(pos)
{
    public override IActionCommand TakeTurn()
    {
        return new WalkAction(this, Global.GetRandomValidDirection());
    }
}
