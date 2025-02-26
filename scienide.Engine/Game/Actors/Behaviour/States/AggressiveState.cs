using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

namespace scienide.Engine.Game.Actors.Behaviour.States;

internal class AggressiveState : StateBase
{
    public AggressiveState(IActor actor) : base(actor)
    {
        State = MonsterState.Aggressive;
    }

    public override IActionCommand Act(Cell[] cells)
    {
        var target = cells.Where(x => x.Actor?.Id == Global.HeroId).FirstOrDefault();
        if (target != null)
        {
            var dir = Direction.GetDirection(Actor.Position, target.Position);
            return new WalkAction(Actor, dir);
        }

        return new WalkAction(Actor, Utils.GetRandomValidDirection());
    }
}
