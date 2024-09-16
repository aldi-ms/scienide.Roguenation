using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public class Actor : GameComposite, IActor
{
    private const string _name = "ActorX";
    private readonly ITimedEntity _timedEntity;

    public ITimedEntity TimedEntity => _timedEntity;
    public string Name => _name;

    public Actor()
    {
        _timedEntity = new DefaultEntity();
    }

    public IActionCommand TakeTurn() => TimedEntity.TakeTurn();

}
