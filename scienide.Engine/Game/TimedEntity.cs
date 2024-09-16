using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

namespace scienide.Engine.Game;

public abstract class TimedEntity : ITimedEntity
{
    public Ulid Id { get; } = Ulid.NewUlid();
    public int Energy { get; set; }
    public int Speed { get; set; }
    public int Cost { get; set; }

    public abstract ITimedEntity Prototype { get; }

    public abstract IActionCommand TakeTurn();
}

public class BaseEntity : TimedEntity
{
    private readonly static BaseEntity _proto = new BaseEntity();
    
    public override ITimedEntity Prototype => _proto;

    public override IActionCommand TakeTurn()
    {
        return new RestAction();
    }
}