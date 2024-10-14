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

    public new Point Position
    {
        get
        {
            return base.Position;
        }
        set
        {
            if (Parent is Cell cell)
            {
                GameMap.DirtyCells.Add(cell);
                cell.RemoveChild(this);
            }
            else
            {
                throw new ArgumentException(nameof(Parent));
            }

            base.Position = value;
            var newPosCell = GameMap.Data[base.Position];
            newPosCell.AddChild(this);
            GameMap.DirtyCells.Add(newPosCell);
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
