namespace scienide.Engine.Game.Actors.Behaviour;

public enum MonsterState
{
    Idle,
    Resting,
    Patrol,
    Aggressive,
    Pursuit,
    Frightened,
    Flee,
    Attacking,
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
    Rested,
    DetectedTarget,
    TargetDead,
    TargetRunning,
    TargetLost,
    TargetInRange,
    Tired, // Hungry?
}
