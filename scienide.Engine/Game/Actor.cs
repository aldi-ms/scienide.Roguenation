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
    public ITimedEntity TimedEntity { get; set; }

    public Actor(string name, Point pos, Glyph glyph, ITimedEntity timedEntity)
        : base(pos)
    {
        _id = Ulid.NewUlid();
        _name = name;
        Glyph = glyph;
        TimedEntity = timedEntity;
    }

    public Actor(Point pos)
        : this(string.Empty, pos, null, null)
    {
    }

    public abstract IActionCommand TakeTurn();
}
