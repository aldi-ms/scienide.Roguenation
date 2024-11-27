namespace scienide.Common.Game;

using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;

public class Cell(Point pos) : GameComposite(pos)
{
    private readonly BitProperties _properties = new();
    private IGameMap? _parentMap = null;

    private Terrain _terrain;

    public IGameMap Map
    {
        get
        {
            if (_parentMap == null)
            {
                if (Parent is IGameMap parentMap)
                {
                    _parentMap = parentMap;
                }
                else
                {
                    throw new ArgumentException(nameof(Parent));
                }
            }

            return _parentMap;
        }
    }

    public IActor? Actor
    {
        get
        {
            if (TryGetComponent(GObjType.ActorPlayerControl | GObjType.ActorNonPlayerControl, out IActor? actorComponent))
            {
                return actorComponent;
            }

            return null;
        }
        set
        {
            if (value != null)
            {
                if (!TryGetComponent(GObjType.ActorPlayerControl | GObjType.ActorNonPlayerControl, out IActor? actorComponent))
                {
                    AddChild(value);
                }
            }
        }
    }

    public new Glyph Glyph
    {
        get
        {
            if (Children == null || Children.Count == 0)
            {
                return base.Glyph;
            }

            var highestOrderElement = Children.OrderByDescending(x => x.Layer).First();
            return highestOrderElement.Glyph;
        }
    }

    public Terrain Terrain
    {
        get { return _terrain; }
        set
        {
            _terrain = value;

            var foundChild = Children.SingleOrDefault(x => x.Layer == CollisionLayer.Terrain);
            if (foundChild != null)
            {
                RemoveChild(foundChild);
            }

            AddChild(_terrain);

            // TODO:
            _properties[Props.IsOpaque] = _terrain.Glyph.Char == '#';
        }
    }

    public BitProperties Properties => _properties;

    public bool IsValidForEntry(GObjType ofType)
    {
        /// TODO
        return Glyph == '.' || Glyph == ',' || Glyph == ' ';
    }

    public Cell[] GetValidNeighbors(Func<Cell, bool>? exclusionFilter = null)
    {
        List<Cell> neighborCells = [];
        for (int dX = -1; dX <= 1; dX++)
        {
            for (int dY = -1; dY <= 1; dY++)
            {
                if (dX == 0 && dY == 0)
                    continue;

                var x = Position.X + dX;
                var y = Position.Y + dY;
                if (!Map.IsInValidMapBounds((x, y)) || !Map[x, y].IsValidForEntry(GObjType.CodeEntry) || (exclusionFilter != null && exclusionFilter(Map[x, y])))
                {
                    continue;
                }

                neighborCells.Add(Map[x, y]);
            }
        }

        return [.. neighborCells];
    }
}
