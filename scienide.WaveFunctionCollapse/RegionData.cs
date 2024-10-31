namespace scienide.WaveFunctionCollapse;

using SadRogue.Primitives;
using scienide.Common.Infrastructure;

public class RegionData
{
    public Ulid Id { get; }
    private FlatArray<char> _map;

    public Dictionary<Direction.Types, char[]> Sides { get; private set; }
    public FlatArray<char> Map
    {
        get { return _map; }
        set
        {
            _map = value;
            Sides = GetMapEdges();
        }
    }

    public bool IsCollapsed => ReferencedRegion != Ulid.Empty;
    public int Entropy => Options.Count;
    public Ulid ReferencedRegion => Options.Count == 1 ? Options[0] : Ulid.Empty;

    public Point GridCoordinates { get; set; }
    public List<Ulid> Options { get; set; }

    public RegionData(FlatArray<char> map, Point gridCoordinates)
    {
        Id = Ulid.NewUlid();
        _map = map ?? throw new ArgumentNullException(nameof(map));
        Sides = GetMapEdges();
        Options = new List<Ulid>(8);
        GridCoordinates = gridCoordinates;
    }

    private char[] GetEdgeCells(Direction dir)
    {
        var xIsMoving = dir.DeltaY != 0;
        var endBound = xIsMoving ? Map.Width : Map.Height;
        // TODO: redo this horror
        var stableAxis = xIsMoving ? dir.DeltaY == 1 ? Map.Height - 1 : 0 : dir.DeltaX == 1 ? Map.Width - 1 : 0;

        char[] result = new char[xIsMoving ? Map.Width : Map.Height];
        for (var a = 0; a < endBound; a++)
        {
            result[a] = xIsMoving ? Map[a, stableAxis] : Map[stableAxis, a];
        }

        return result;
    }

    private Dictionary<Direction.Types, char[]> GetMapEdges()
    {
        return new Dictionary<Direction.Types, char[]>
        {
            { Direction.Up, GetEdgeCells(Direction.Up) },
            { Direction.Down, GetEdgeCells(Direction.Down) },
            { Direction.Left, GetEdgeCells(Direction.Left) },
            { Direction.Right, GetEdgeCells(Direction.Right) },
        };
    }
}