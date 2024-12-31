namespace scienide.Engine.Game.Actors.Behaviour.States;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

internal class IdleState : StateBase
{
    public IdleState(IActor actor) : base(actor)
    {
        State = MonsterState.Idle;
    }

    public override IActionCommand Act(Cell[] cells)
    {
        return NoneAction.Instance;
    }
}
