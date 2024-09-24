using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public class Actor : GameComposite, IActor
{
    private readonly static string _name = Ulid.NewUlid().ToString();

    private readonly ITimedEntity _timedEntity;

    public ITimedEntity TimedEntity => _timedEntity;
    public string Name => _name;

    public Actor(Point pos, Glyph glyph, ITimedEntity timedEntity) : base(pos)
    {
        Glyph = glyph;
        _timedEntity = timedEntity;
    }

    public IActionCommand TakeTurn() => TimedEntity.TakeTurn();

}
