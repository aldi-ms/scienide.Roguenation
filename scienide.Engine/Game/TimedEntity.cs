using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

namespace scienide.Engine.Game;

public abstract class TimedEntity : ITimedEntity
{
    public int Energy { get; set; }
    public int Speed { get; set; }
    public int Cost { get; set; }

    public abstract IActionCommand TakeTurn();
}

public class DefaultTimedEntity : TimedEntity
{
    public DefaultTimedEntity(IGameComponent parent)
    {
    }

    public override IActionCommand TakeTurn()
    {
        return new RestAction(null);
    }
}