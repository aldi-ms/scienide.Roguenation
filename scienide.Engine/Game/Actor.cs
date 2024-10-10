using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public abstract class Actor : GameComposite, IActor
{
    private readonly Ulid _id;
    private readonly string _name;

    public string Name => _name;
    public Ulid Id => _id;
    public ITimedEntity? TimedEntity { get; set; }
    public IActionCommand? Action { get; set; }

    public Actor(Point pos)
        : base(pos)
    {
        _id = Ulid.NewUlid();
        _name = string.Empty;
        Layer = CollisionLayer.Actor;
    }

    public abstract IActionCommand TakeTurn();
}
