using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public abstract class Actor : GameComposite, IActor
{
    private readonly Ulid _id;
    private readonly string _name;
    private readonly ITimedEntity _timedEntity;

    public ITimedEntity TimedEntity => _timedEntity;
    public string Name => _name;
    public Ulid Id => _id;

    public Actor(string name, Point pos, Glyph glyph, ITimedEntity timedEntity) 
        : base(pos)
    {
        _id = Ulid.NewUlid();
        _name = name;
        _timedEntity = timedEntity;
        Glyph = glyph;
    }

    public IActionCommand TakeTurn()
    {
        return TimedEntity.TakeTurn(this);
    }

}
