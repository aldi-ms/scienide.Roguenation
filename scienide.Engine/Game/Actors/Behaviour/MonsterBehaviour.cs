namespace scienide.Engine.Game.Actors.Behaviour;

using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Components;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;
using scienide.Engine.Game.Actors.Behaviour.States;
using Stateless;
using Stateless.Graph;
using System.Diagnostics;

internal class MonsterBehaviour : BehaviourBase
{
    public const string AILoggingFolder = "AILogging";

    private StateBase _currentState;
    private readonly StateMachine<MonsterState, MonsterTrigger> _stateMachine;

    // TODO: Make separate behaviour log


    public MonsterBehaviour(IActor actor) : base(actor)
    {
        VisibleCells = [];
        _stateMachine = new StateMachine<MonsterState, MonsterTrigger>(MonsterState.Idle);

        _stateMachine.Configure(MonsterState.Idle)
            .OnEntry(() => Trace.WriteLine($"{Actor.Name} just hangs around."))
            .Permit(MonsterTrigger.HealthLow, MonsterState.Resting)
            .Permit(MonsterTrigger.HealthCritical, MonsterState.Resting)
            .Permit(MonsterTrigger.Rested, MonsterState.Patrol)
            .Permit(MonsterTrigger.DetectedTarget, MonsterState.Aggressive);

        _stateMachine.Configure(MonsterState.Patrol)
            .OnEntry(() => Trace.WriteLine($"{Actor.Name} starts patrolling."))
            .Permit(MonsterTrigger.DetectedTarget, MonsterState.Aggressive)
            .Permit(MonsterTrigger.Tired, MonsterState.Resting);

        //_stateMachine.Configure(MonsterState.Resting)
        //    .OnEntry(() => Console.WriteLine($"{Actor.Name} is resting."))
        //    .Permit(MonsterTrigger.Rested, MonsterState.Patrol)
        //    .Permit(MonsterTrigger.DetectedTarget, MonsterState.Aggressive);

        //_stateMachine.Configure(MonsterState.Frightened)
        //    .OnEntry(() => Console.WriteLine($"{Actor.Name} is frightened."))
        //    .Permit(MonsterTrigger.Rested, MonsterState.Idle)
        //    .Permit(MonsterTrigger.HealthCritical, MonsterState.Flee)
        //    .Permit(MonsterTrigger.TargetInRange, MonsterState.Attacking)
        //    .Permit(MonsterTrigger.TargetTooFar, MonsterState.Resting)
        //    .Permit(MonsterTrigger.TargetRunning, MonsterState.Resting);

        _stateMachine.Configure(MonsterState.Aggressive)
            .OnEntry(() => Trace.WriteLine($"{Actor.Name} is getting aggressive."))
            .Permit(MonsterTrigger.HealthLow, MonsterState.Frightened)
            .Permit(MonsterTrigger.HealthCritical, MonsterState.Flee)
            .Permit(MonsterTrigger.TargetDead, MonsterState.Resting)
            .Permit(MonsterTrigger.TargetRunning, MonsterState.Pursuit)
            .Permit(MonsterTrigger.TargetLost, MonsterState.Patrol)
            .Permit(MonsterTrigger.TargetInRange, MonsterState.Attacking);

        //_stateMachine.Configure(MonsterState.Attacking)
        //    .OnEntry(() => Console.WriteLine($"{Actor.Name} is attacking."))
        //    .Permit(MonsterTrigger.HealthLow, MonsterState.Frightened)
        //    .Permit(MonsterTrigger.HealthCritical, MonsterState.Flee)
        //    .Permit(MonsterTrigger.TargetDead, MonsterState.Resting)
        //    .Permit(MonsterTrigger.TargetRunning, MonsterState.Pursuit);

        //_stateMachine.Configure(MonsterState.Flee)
        //    .OnEntry(() => Console.WriteLine($"{Actor.Name} is fleeing."))
        //    .Permit(MonsterTrigger.Tired, MonsterState.Resting)
        //    .Permit(MonsterTrigger.TargetTooFar, MonsterState.Resting);

        //_stateMachine.Configure(MonsterState.Pursuit)
        //    .OnEntry(() => Console.WriteLine($"{Actor.Name} is in active pursuit."))
        //    .Permit(MonsterTrigger.TargetTooFar, MonsterState.Patrol)
        //    .Permit(MonsterTrigger.TargetDead, MonsterState.Idle)
        //    .Permit(MonsterTrigger.TargetInRange, MonsterState.Attacking);

        _stateMachine.OnUnhandledTrigger((state, trigger) =>
        {
            Trace.WriteLine($"Unhandled state machine state/trigger: {state}/{trigger}!");
        });


        _currentState = StateFactory.CreateState(_stateMachine.State, Actor);
        //LogDotUmlToFile();
    }

    public Cell[] VisibleCells { get; set; }

    public override string Status => _currentState.State.ToString();

    public override IActionCommand Act()
    {
        if (Actor.GameMap == null)
        {
            // TODO: Fetching the map like that does not look/work well...
            Trace.WriteLine($"Actor {Actor.Name} game map is null!");
            return new WalkAction(Actor, Utils.GetRandomValidDirection());
        }

#if ENABLE_FOV
        VisibleCells = [.. Actor.GameMap.FoV.Compute(Actor.Position, Actor.FoVRange)];
#else
        VisibleCells = [.. MapUtils.GetCellsWithinDistance(Actor.GameMap, Actor.Position, Actor.FoVRange)];
#endif

        //VisibleCells = Global.EnableFov
        //    ? [.. Actor.GameMap.FoV.Compute(Actor.Position, Actor.FoVRange)]
        //    : [.. MapUtils.GetCellsWithinDistance(Actor.GameMap, Actor.Position, Actor.FoVRange)];
        EvaluateState();

        return _currentState.Act(VisibleCells);
    }

    public override void EvaluateState()
    {
        var targetCell = VisibleCells.Where(x => x.Actor?.TypeId == Global.HeroId).FirstOrDefault();
        if (targetCell != null && _stateMachine.CanFire(MonsterTrigger.DetectedTarget))
        {
            _stateMachine.Fire(MonsterTrigger.DetectedTarget);
        }
        else if (_stateMachine.CanFire(MonsterTrigger.Rested))
        {
            _stateMachine.Fire(MonsterTrigger.Rested);
        }
        if (_stateMachine.State == MonsterState.Aggressive && targetCell == null)
        {
            _stateMachine.Fire(MonsterTrigger.TargetLost);
        }


        if (_stateMachine.State != _currentState.State)
        {
            _currentState = StateFactory.CreateState(_stateMachine.State, Actor);
        }
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