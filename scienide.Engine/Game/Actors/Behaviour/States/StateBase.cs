using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

namespace scienide.Engine.Game.Actors.Behaviour.States;

public abstract class StateBase(IActor actor)
{
    public MonsterState State { get; protected set; }

    public IActor Actor { get; set; } = actor;

    public abstract IActionCommand Act(Cell[] cells);
}
