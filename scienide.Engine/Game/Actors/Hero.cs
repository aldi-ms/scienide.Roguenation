namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

public class Hero(Point pos) : Actor(pos)
{
    public override IActionCommand TakeTurn()
    {
        if (Action == null)
        {
            // TODO: ?
            return new RestAction(this);
        }

        return Action;
    }
}
