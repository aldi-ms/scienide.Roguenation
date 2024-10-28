namespace scienide.Engine.Game.Actions;

using SadRogue.Primitives;
using scienide.Common.Messaging.Events;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Core.Messaging;
using scienide.Engine.Infrastructure;

public class WalkAction(IActor? actor, Direction dir) : ActionCommand(actor, 100, "Walk action", "{0} walked {1}.")
{
    private const string GameMessageStyle = "[c:r f:green]";

    private readonly Direction _direction = dir;

    public override int Execute()
    {
        if (Actor == null)
        {
            throw new ArgumentNullException(nameof(Actor));
        }

        var newPosition = Actor.Position + _direction;
        if (newPosition.X < 0 || newPosition.X >= Actor.GameMap.Width
            || newPosition.Y < 0 || newPosition.Y >= Actor.GameMap.Height
            || !Actor.GameMap[newPosition].IsValidForEntry(GObjType.ActorPlayerControl))
        {
            var message = GameMessageStyle + string.Format(Description, Actor.Name, "straight into a wall.");

            MessageBroker.Instance.Broadcast(new GameMessageArgs(Actor.Position, message, ushort.MaxValue));

            return 0;
        }

        Actor.Position = newPosition;

        return Cost;
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
