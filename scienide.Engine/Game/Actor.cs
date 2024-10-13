using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public abstract class Actor : GameComposite, IActor
{
    private readonly Ulid _id;
    private readonly string _name;
    private ITimeEntity? _timeEntity;
    
    public string Name => _name;
    public Ulid Id => _id;
    public ITimeEntity? TimeEntity
    {
        get { return _timeEntity; }
        set
        {
            _timeEntity = value;
            if (_timeEntity != null)
            {
                _timeEntity.Actor = this;
            }
        }
    }
    public IActionCommand? Action { get; set; }

    public Actor(Point pos) : base(pos)
    {
        _id = Ulid.NewUlid();
        _name = string.Empty;
        Layer = CollisionLayer.Actor;
    }

    public abstract IActionCommand TakeTurn();
}
