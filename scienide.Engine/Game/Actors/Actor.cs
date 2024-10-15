namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

public abstract class Actor : GameComposite, IActor
{
    private readonly Ulid _id;
    private readonly string _name;
    private ITimeEntity? _timeEntity;
    private IGameMap? _map;

    public Actor(Point pos) : base(pos)
    {
        _id = Ulid.NewUlid();
        _name = string.Empty;
        Layer = CollisionLayer.Actor;
    }

    public string Name => _name;

    public Ulid Id => _id;

    public IActionCommand? Action { get; set; }

    private Cell CurrentCell => GameMap.Data[Position];

    private IGameMap GameMap
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
            /// TODO: Validate position before assigning, checking for out-of-bounds
            GameMap.DirtyCells.Add(CurrentCell);
            CurrentCell.RemoveChild(this);

            base.Position = value;

            CurrentCell.AddChild(this);
            GameMap.DirtyCells.Add(CurrentCell);
        }
    }

    public abstract IActionCommand TakeTurn();
}
