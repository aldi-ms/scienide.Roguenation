namespace scienide.Engine.Map;

using scienide.Common.Game;
using scienide.Common.Map;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

internal class MapNode(RegionCellData region)
{
    public RegionCellData Region { get; } = region;
    public Dictionary<Ulid, MapConnection> Connections { get; } = [];
}

internal class MapConnection(MapNode node)
{
    public MapNode Node { get; set; } = node;
    public HashSet<Cell> BorderingCells { get; set; } = [];
}

internal class MapGraph
{
    private readonly Dictionary<Ulid, MapNode> _mapNodes = [];

    public void AddNode(MapNode node)
    {
        if (_mapNodes.ContainsKey(node.Region.Id))
        {
            Trace.WriteLine($"Unexpected! Map tree already contains node with id: {node.Region.Id}!");
        }

        _mapNodes.Add(node.Region.Id, node);
    }

    public bool TryGetNode(Ulid id, [MaybeNullWhen(false)] out MapNode node)
    {
        return _mapNodes.TryGetValue(id, out node);
    }
}

