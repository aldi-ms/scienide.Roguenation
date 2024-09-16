using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public class Actor : GameComposite, IActor
{
    private readonly ITimedEntity _timedEntity;
    public ITimedEntity TimedEntity => _timedEntity;

    public Actor()
    {
        _timedEntity = new BaseEntity();
    }

    public IActionCommand TakeTurn() => TimedEntity.TakeTurn();

}
