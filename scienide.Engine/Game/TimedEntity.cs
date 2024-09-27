using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

namespace scienide.Engine.Game;

public abstract class TimedEntity(IActor? actor) : ITimedEntity
{
    public int Energy { get; set; }
    public int Speed { get; set; }
    public int Cost { get; set; }
    public IActor? Actor { get; } = actor;

    public abstract IActionCommand TakeTurn();
}

public class HeroTimedEntity(IActor actor) : TimedEntity(actor)
{
    public override IActionCommand TakeTurn()
    {
        // Handle input to return an action
        return new RestAction(Actor ?? throw new ArgumentNullException(nameof(Actor)));
    }
}