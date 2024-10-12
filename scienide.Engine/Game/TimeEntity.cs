namespace scienide.Engine.Game;

using scienide.Engine.Core.Interfaces;

public abstract class TimeEntity(IActor? actor) : ITimeEntity
{
    public int Energy { get; set; }
    public int Speed { get; set; }
    public int Cost { get; set; }
    public IActor? Actor { get; } = actor;

    public abstract IActionCommand TakeTurn();
}

public class HeroTimeEntity(IActor actor) : TimeEntity(actor)
{
    public override IActionCommand TakeTurn()
    {
        // Handle input to return an action
        return Actor?.TakeTurn() ?? throw new ArgumentNullException(nameof(Actor));
    }
}