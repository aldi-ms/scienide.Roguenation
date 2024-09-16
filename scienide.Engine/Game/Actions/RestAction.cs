using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game.Actions;

public class RestAction : ActionCommand
{
    private const string ActionName = "Rest action";
    private const string ActionDescription = "{0} took a rest turn.";

    public RestAction()
        : base(ActionName, ActionDescription)
    {
    }

    public override int Execute()
    {
        return 0;
    }

    public override void Undo()
    {
        // Don't do anything
    }

    public string GetActionLogText(IActor actor)
    {
        /// TODO: Move action log code to abstract class
        return string.Format(ActionDescription, actor.Name);
    }
}
