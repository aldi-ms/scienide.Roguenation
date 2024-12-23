namespace scienide.Common.Game;

using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;

public class Cell : GameComposite, IGenericCloneable<Cell>
{
    private BitProperties _properties = new();
    private IGameMap? _parentMap = null;

    private Terrain _terrain;

    public Cell(Point pos) : base(pos)
    {
        ObjectType = GObjType.Cell;
    }

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
            if (TryGetComponent(GObjType.Player | GObjType.NPC, out IActor? actorComponent))
            {
                return actorComponent;
            }

            return null;
        }
        set
        {
            if (value != null)
            {
                if (!TryGetComponent(GObjType.Player | GObjType.NPC, out IActor? actorComponent))
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
            var resultGlyph = highestOrderElement.Glyph;
            if ((highestOrderElement.ObjectType & (GObjType.Player | GObjType.NPC)) != 0)
            {
                resultGlyph.Appearance.Background = Children.Where(x => x.ObjectType == GObjType.Terrain).Single().Glyph.Appearance.Background;
            }

            return resultGlyph;
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

            /// TODO:
            _properties[Props.IsOpaque] = _terrain.Glyph.Char == '#';
        }
    }

    public BitProperties Properties => _properties;

    public bool IsValidForEntry(GObjType ofType)
    {
        return ofType switch
        {
            _ when (ofType & (GObjType.NPC | GObjType.Player)) != 0 => Glyph == '.' || Glyph == ',' || Glyph == ' ',
            _ => true
        };
    }

    public Cell[] GetValidNeighbours(Func<Cell, bool>? exclusionFilter = null)
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

                if (!Map.IsInValidMapBounds(x, y)
                    || (exclusionFilter != null && exclusionFilter(Map[x, y])))
                {
                    continue;
                }

                neighborCells.Add(Map[x, y]);
            }
        }

        return [.. neighborCells];
    }

    public Cell Clone(bool deepClone)
    {
        var cell = new Cell(Position);
        cell.Terrain = new Terrain(Terrain.Glyph.Clone(deepClone));
        if (Actor != null)
        {
            cell.Actor = Actor.Clone(deepClone);
        }

        return cell;
    }

    public override string ToString()
    {
        return Position.ToString();
    }
}
