using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using System.Diagnostics;

namespace scienide.Engine.Game.Actions;

public class WalkAction : ActionCommand
{
    private readonly Direction _direction;

    public WalkAction(IActor? actor, Direction dir)
        : base(actor, 100, "Walk action", "{0} walked {1}.")
    {
        _direction = dir;
    }

    public override int Execute()
    {
        if (Actor == null)
        {
            throw new ArgumentNullException(nameof(Actor));
        }

        var newPosition = Actor.Position + _direction;
        if (newPosition.X < 0 || newPosition.X >= Actor.GameMap.Width
            || newPosition.Y < 0 || newPosition.Y >= Actor.GameMap.Height)
        {
            Trace.WriteLine(string.Format(Description, Actor.Name, "straight into a wall."));
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
