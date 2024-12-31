using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actors.Behaviour.States;

namespace scienide.Engine.Game.Actors.Behaviour;

internal static class StateFactory
{
    internal static StateBase CreateState(MonsterState state, IActor actor)
    {
        return state switch
        {
            MonsterState.Idle => new IdleState(actor),
            MonsterState.Aggressive => new AggressiveState(actor),
            MonsterState.Patrol => new PatrolState(actor),
            _ => throw new ArgumentException($"{nameof(StateFactory)} could not create state for {state}!", nameof(state))
        };
    }
}
