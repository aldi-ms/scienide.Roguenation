namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using System.Diagnostics;

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

    public new Point Position
    {
        get
        {
            return base.Position;
        }
        set
        {
            GameMap.DirtyCells.Add(CurrentCell);

            CurrentCell.RemoveChild(this);

            base.Position = value;
            CurrentCell.AddChild(this);

            GameMap.DirtyCells.Add(CurrentCell);
        }
    }

    private Cell CurrentCell => GameMap[Position];

    public abstract IActionCommand TakeTurn();

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
        Trace.WriteLine($"[{Name}] can hear: {args.Message}.");
    }
}
