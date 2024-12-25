namespace scienide.Engine.Game.Actors.Behaviour;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;
using Stateless;
using Stateless.Graph;

public class MonsterAI : BaseAI
{
    public const string AILoggingFolder = "AILogging";

    private readonly StateMachine<MonsterState, MonsterTrigger> _stateMachine;

    public MonsterAI(IActor actor) : base(actor)
    {
        _stateMachine = new StateMachine<MonsterState, MonsterTrigger>(MonsterState.Idle);

        _stateMachine.Configure(MonsterState.Idle)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} just hangs around."))
            .Permit(MonsterTrigger.HealthLow, MonsterState.Resting)
            .Permit(MonsterTrigger.HealthCritical, MonsterState.Resting)
            .Permit(MonsterTrigger.Rested, MonsterState.Patrol)
            .Permit(MonsterTrigger.DetectedTarget, MonsterState.Aggressive);

        _stateMachine.Configure(MonsterState.Patrol)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} starts patrolling."))
            .Permit(MonsterTrigger.DetectedTarget, MonsterState.Aggressive)
            .Permit(MonsterTrigger.Tired, MonsterState.Resting);

        _stateMachine.Configure(MonsterState.Resting)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is resting."))
            .Permit(MonsterTrigger.Rested, MonsterState.Patrol)
            .Permit(MonsterTrigger.DetectedTarget, MonsterState.Aggressive);

        _stateMachine.Configure(MonsterState.Frightened)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is frightened."))
            .Permit(MonsterTrigger.Rested, MonsterState.Idle)
            .Permit(MonsterTrigger.HealthCritical, MonsterState.Flee)
            .Permit(MonsterTrigger.TargetInRange, MonsterState.Attacking)
            .Permit(MonsterTrigger.TargetTooFar, MonsterState.Resting)
            .Permit(MonsterTrigger.TargetRunning, MonsterState.Resting);

        _stateMachine.Configure(MonsterState.Aggressive)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is feeling aggressive."))
            .Permit(MonsterTrigger.HealthLow, MonsterState.Frightened)
            .Permit(MonsterTrigger.HealthCritical, MonsterState.Flee)
            .Permit(MonsterTrigger.TargetDead, MonsterState.Resting)
            .Permit(MonsterTrigger.TargetRunning, MonsterState.Pursuit)
            .Permit(MonsterTrigger.TargetTooFar, MonsterState.Patrol)
            .Permit(MonsterTrigger.TargetInRange, MonsterState.Attacking);

        _stateMachine.Configure(MonsterState.Attacking)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is attacking."))
            .Permit(MonsterTrigger.HealthLow, MonsterState.Frightened)
            .Permit(MonsterTrigger.HealthCritical, MonsterState.Flee)
            .Permit(MonsterTrigger.TargetDead, MonsterState.Resting)
            .Permit(MonsterTrigger.TargetRunning, MonsterState.Pursuit);

        _stateMachine.Configure(MonsterState.Flee)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is fleeing."))
            .Permit(MonsterTrigger.Tired, MonsterState.Resting)
            .Permit(MonsterTrigger.TargetTooFar, MonsterState.Resting);

        _stateMachine.Configure(MonsterState.Pursuit)
            .OnEntry(() => Console.WriteLine($"{Actor.Name} is in active pursuit."))
            .Permit(MonsterTrigger.TargetTooFar, MonsterState.Patrol)
            .Permit(MonsterTrigger.TargetDead, MonsterState.Idle)
            .Permit(MonsterTrigger.TargetInRange, MonsterState.Attacking);

        //LogDotUmlToFile();
    }

    public override IActionCommand Act()
    {
        if (Actor.GameMap == null)
        {
            return new WalkAction(Actor, Utils.GetRandomValidDirection());
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

        return new WalkAction(Actor, Utils.GetRandomValidDirection());
    }

    private Cell? GetTarget(List<Cell> cells)
    {
        return cells.Where(x => x.Actor?.TypeId == Global.HeroId).FirstOrDefault();
    }

    private void LogDotUmlToFile()
    {
        var dotUml = UmlDotGraph.Format(_stateMachine.GetInfo());
        var fName = $"{string.Join(string.Empty, Actor.Name.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)))}.dot";
        var fullFilePath = Path.Combine(AILoggingFolder, fName);
        if (File.Exists(fullFilePath))
        {
            var archiveDir = Path.Combine(AILoggingFolder, "Archive");
            if (!Directory.Exists(archiveDir))
            {
                Directory.CreateDirectory(archiveDir);
            }

            File.Move(fullFilePath, Path.Combine(archiveDir, $"{Global.RNG.Next()}.{fName}"));
        }

        Utils.WriteToFile(fullFilePath, dotUml);
    }
}