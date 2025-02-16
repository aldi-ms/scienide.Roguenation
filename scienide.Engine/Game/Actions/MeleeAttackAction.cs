namespace scienide.Engine.Game.Actions;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Components;
using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging;

public class MeleeAttackAction(IActor actor, Point target)
    : ActionCommandBase(actor, 100, "Melee Attack", "{0} takes a close-quarters swing at {1}.")
{
    private readonly Point _target = target;

    public override ActionResult Execute()
    {
        ArgumentNullException.ThrowIfNull(Actor);

        var map = Actor.GameMap;
        var targetCell = map[_target];

        ArgumentNullException.ThrowIfNull(targetCell.Actor);
        // We have acquired a target
        var targetActor = targetCell.Actor;

        if (Actor.TypeId == Global.HeroId)
        {
            MessageBroker.Instance.Broadcast(new SelectedCellChanged(targetCell));
        }

        map.GameLogger.Information(Description, Actor.Name, targetActor.Name);
        if (!Actor.TryGetComponent<AttackComponent>(out var atk, true))
        {
            map.GameLogger.Information($"{Actor.Name} cannot attack at the moment.");
            return ActionResult.Fail();
        }

        if (!targetActor.TryGetComponent<DefenseComponent>(out var targetDefense, true))
        {
            map.GameLogger.Warning($"Unexpected, actor {targetActor.Name} does not have a defense component!");
            return ActionResult.Fail();
        }

        if (!targetActor.TryGetComponent<StatsComponent>(out var targetStats, true))
        {
            map.GameLogger.Warning($"Unexpected, actor {targetActor.Name} does not have a defense component!");
            return ActionResult.Fail();
        }

        var outgoingAttackDamage = atk.Attack();
        var ingoingDamage = outgoingAttackDamage - targetDefense.Defense;

        map.DirtyCells.Add(targetCell);
        map.DirtyCells.Add(Actor.CurrentCell);

        targetStats.TakeDamage(ingoingDamage);

        return ActionResult.Success(Cost);
    }

    public override string GetActionLog()
    {
        throw new NotImplementedException();
    }

    public override void Undo()
    {
        throw new NotImplementedException();
    }
}
