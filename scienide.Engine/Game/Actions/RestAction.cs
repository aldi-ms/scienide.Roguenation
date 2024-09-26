using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game.Actions;

public class RestAction : ActionCommand
{
    public RestAction(IActor actor)
        : base(actor, 100, "Rest action", "{0} spent a turn to rest.")
    {
    }

    public override int Execute()
    {
        // For now just return the ActionCost
        return Cost;
    }

    public override void Undo()
    {
        // Don't do anything
    }

    public override string GetActionLog()
    {
        /// TODO: Move action log code to abstract class
        return string.Format(Description, Actor?.Name ?? "The actor");
    }
}
