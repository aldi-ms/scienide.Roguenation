﻿namespace scienide.Engine.Game.Actions;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using System.Diagnostics;

public class WalkAction(IActor? actor, Direction dir) : ActionCommandBase(actor, 100, "Walk action", "{0} walked {1} {2}.")
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
            || !Actor.GameMap[newPosition].IsValidForEntry(GObjType.Player))
        {
            var message = GameMessageStyle + string.Format(Description, Actor.Name, _direction.ToString().ToLowerInvariant(), $"straight into a wall at {Actor.Position}.");
            MessageBroker.Instance.Broadcast(new GameMessageArgs(Actor.Position, message, 7));

            return 0;
        }

        Trace.WriteLine($"{DateTime.Now:O} Executing {nameof(WalkAction)} for {Actor.Name} from {Actor.Position} to {newPosition}.");

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
