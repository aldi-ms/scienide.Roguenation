namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;

public abstract class Actor : GameComposite, IActor
{
    private Ulid _id;
    private string _name;
    private ITimeEntity? _timeEntity;
    private IGameMap? _map;

    public Actor(Point pos, string name) : base(pos)
    {
        _id = Ulid.NewUlid();
        _name = name;
        Layer = CollisionLayer.Actor;
    }

    public Actor(Point pos) : this(pos, string.Empty)
    {
    }

    public new Point Position
    {
        get
        {
            return base.Position;
        }
        set
        {
            if (base.Position == value)
            {
                GameMap.GameLogger.Warning("Trying to set {Name}'s position to the same value: {Position}.", Name, Position);
                return;
            }

            var oldCell = GameMap[Position];
            GameMap.DirtyCells.Add(oldCell);

            oldCell.RemoveComponent(this);

            base.Position = value;
            var newCell = GameMap[Position];
            newCell.AddComponent(this);

            GameMap.DirtyCells.Add(newCell);
        }
    }

    public string Name
    {
        get => _name;
        internal set => _name = value;
    }

    public Ulid TypeId
    {
        get => _id;
        protected set => _id = value;
    }

    public IActionCommand? Action { get; set; }

    public IGameMap GameMap
    {
        get
        {
            if (_map != null)
            {
                return _map;
            }

            if (Parent?.Parent is IGameMap map)
            {
                _map = map;
                return _map;
            }

            throw new ArgumentNullException(nameof(Parent));
        }
    }

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

    public int FoVRange { get; set; }

    public Cell CurrentCell => GameMap[Position];

    public abstract IActionCommand TakeTurn();

    public abstract IActor Clone(bool deepClone);

    public virtual void SubscribeForMessages()
    {
        MessageBroker.Instance.Subscribe<GameMessageArgs>(Listener, this);
    }

    public virtual void UnsubscribeFromMessages()
    {
        MessageBroker.Instance.Unsubscribe<GameMessageArgs>(Listener, this);
    }

    private void Listener(GameMessageArgs args)
    {
        GameMap.GameLogger.Information("[{Name}] can hear message: {@args}.", Name, args);
    }
}
