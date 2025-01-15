namespace scienide.Engine.Game.Actions;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;

public class WalkAction(IActor? actor, Direction dir) : ActionCommandBase(actor, 100, "Walk action", "{0} walked {1} {2}.")
{
    private const string GameMessageStyle = "[c:r f:green]";

    private readonly Direction _direction = dir;

    public override ActionResult Execute()
    {
        if (Actor == null)
        {
            throw new ArgumentNullException(nameof(Actor));
        }

        var newPosition = Actor.Position + _direction;
        if (newPosition.X < 0 || newPosition.X >= Actor.GameMap.Width
            || newPosition.Y < 0 || newPosition.Y >= Actor.GameMap.Height)
        {
            // We are out of bounds
            var message = GameMessageStyle + string.Format(Description, Actor.Name, _direction.ToString().ToLowerInvariant(), $"out of bounds at {Actor.Position}.");
            MessageBroker.Instance.Broadcast(new GameMessage(Actor.Position, message, 7));

            return ActionResult.Fail();
        }
        else if (!Actor.GameMap[newPosition].IsValidCellForEntry(GObjType.Player | GObjType.NPC))
        {
            // In bounds
            if (Actor.GameMap[newPosition].Actor != null)
            {
                // We have a target
                return ActionResult.Alternative(new MeleeAttackAction(Actor, newPosition));
            }

            var message = GameMessageStyle + string.Format(Description, Actor.Name, _direction.ToString().ToLowerInvariant(), $"straight into a wall at {Actor.Position}.");
            MessageBroker.Instance.Broadcast(new GameMessage(Actor.Position, message, 7));

            return ActionResult.Fail();
        }

        Actor.GameMap.GameLogger.Information("Executing WalkAction for {Actor} from {Position} to {newPosition}.", Actor.Name, Actor.Position, newPosition);

        Actor.Position = newPosition;

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
