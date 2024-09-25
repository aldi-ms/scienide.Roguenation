using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game.Actions;

public class WalkAction : ActionCommand
{
    private readonly Direction _direction;

    public WalkAction(IActor? actor, Direction dir) 
        : base(actor, "Walk action", "{0} walked {1}.")
    {
        _direction = dir;
    }

    public override int Execute()
    {
        if (Actor == null)
        {
            throw new ArgumentNullException(nameof(Actor));
        }

        Actor.Position += _direction;
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
