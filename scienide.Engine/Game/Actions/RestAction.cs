namespace scienide.Engine.Game.Actions;

using scienide.Common.Game;
using scienide.Common.Game.Components;
using scienide.Common.Game.Interfaces;

public class RestAction(IActor actor) : ActionCommandBase(actor, 100, "Rest action", "{0} spent a turn to rest.")
{
    public override ActionResult Execute()
    {
        ArgumentNullException.ThrowIfNull(Actor);

        if (!Actor.TryGetComponent(out StatsComponent? stats, true) || stats.CurrentHealth >= stats.MaxHealth)
        {
            return ActionResult.Success(0);
        }

        stats.CurrentHealth += 1;

        return ActionResult.Success(Cost);
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
