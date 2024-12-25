namespace scienide.Engine.AI.Behaviour;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;
using Stateless;

public class MonsterAI : BaseAI
{
    public enum MonsterState
    {
        Idle,
        Lazy,
        Patrol,
        Attack,
        Aggressive,
        Frightened,
        Flee,
        // Search, Alert, Defend, Evade, Taunt, Recover
        // Ambush, Follow, Summon, Transform, Sleep
        // Climb, Swim
        // Rally, Retreat, Flank
        // Distracted, Confused, Stunned, Blinded, Captured
    }

    public enum MonsterTrigger
    {
        HealthLow,
        HealthCritical,
        Calm,
        Rested,
        DetectedTarget,
        Irritated
    }

    private readonly StateMachine<MonsterState, MonsterTrigger> _stateMachine;

    public MonsterAI(IActor actor) : base(actor)
    {
        _stateMachine = new StateMachine<MonsterState, MonsterTrigger>(MonsterState.Idle);

        _stateMachine.Configure(MonsterState.Idle)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is feeling tired."))
            .Permit(MonsterTrigger.HealthLow, MonsterState.Lazy);
        _stateMachine.Configure(MonsterState.Lazy)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is feeling lazy."))
            .Permit(MonsterTrigger.HealthLow, MonsterState.Frightened);
        _stateMachine.Configure(MonsterState.Frightened)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is feeling frightened."))
            .Permit(MonsterTrigger.Rested, MonsterState.Lazy);
        _stateMachine.Configure(MonsterState.Aggressive)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is feeling aggressive."))
            .Permit(MonsterTrigger.Calm, MonsterState.Lazy);
    }

    public override IActionCommand Act()
    {
        if (Actor.GameMap == null)
        {
            return new WalkAction(Actor, Global.GetRandomValidDirection());
        }

        Cell[] dirtyCellsCopy = new Cell[Actor.GameMap.DirtyCells.Count];
        Actor.GameMap.DirtyCells.CopyTo(dirtyCellsCopy);

        var visibleCells = Actor.GameMap.FoV.Compute(Actor.Position, Actor.FoVRange);

        // Refactor this with an Actor state machine
        // Attacking/Angry, Lazy, Frightened, Normal
        var target = GetTarget(visibleCells);

        if (target != null)
        {
            var dir = Direction.GetDirection(Actor.Position, target.Position);
            return new WalkAction(Actor, dir);
        }

        return new WalkAction(Actor, Global.GetRandomValidDirection());
    }

    private static Cell? GetTarget(List<Cell> cells)
    {
        return cells.Where(x => x.Actor?.TypeId == Global.HeroId).FirstOrDefault();
    }
}