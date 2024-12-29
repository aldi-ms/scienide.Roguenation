namespace scienide.Common.Game;

using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;

public class Cell : GameComposite, IGenericCloneable<Cell>
{
    private readonly BitProperties _properties = new();
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
            if (TryGetComponent<IActor>(out var actor))
            {
                return actor;
            }

            return null;
        }
        set
        {
            if (value == null)
            {
                Map.GameLogger.Error($"Attempt to assign Actor to null!");
                return;
            }

            if (!TryGetComponent<IActor>(out var _))
            {
                AddComponent(value);
            }
            else
            {
                Map.GameLogger.Warning("Cell {Position} already contains an actor, when trying to set to {@value}.", Position, value);
            }
        }
    }

    public new Glyph Glyph
    {
        get
        {
            if (Components == null || Components.Count == 0)
            {
                return base.Glyph;
            }

            var highestOrderElement = Components.OrderByDescending(x => x.Layer).First();
            var resultGlyph = highestOrderElement.Glyph;
            if ((highestOrderElement.ObjectType & (GObjType.Player | GObjType.NPC)) != 0)
            {
                resultGlyph.Appearance.Background = Components.Where(x => x.ObjectType == GObjType.Terrain).Single().Glyph.Appearance.Background;
            }

            return resultGlyph;
        }
    }

    public Terrain Terrain
    {
        get { return _terrain; }
        set
        {
            RemoveComponent(_terrain);

            _terrain = value;

            AddComponent(_terrain);

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
