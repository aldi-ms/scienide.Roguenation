namespace scienide.Engine.Map;

using SadConsole;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Common.Map;
using scienide.Engine.Game;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

public class FloodFillGeneration
{
    /// <summary>
    /// Make sure all regions are reachable by w/e means possible.
    /// Bigger regions should have at least 2 entries - single/double doors.
    /// Smaller regions should have at least 1 single door entry.
    /// Smallest can have a (hidden) teleport leading inside of them.
    /// </summary>
    /// <param name="mapRegions"></param>
    public static void ConnectMapRegions(List<RegionCellData> mapRegions)
    {
        var orderedRegions = mapRegions.OrderBy(x => x.Cells.Count).ToList();
        var mapGraph = GenerateMapGraph(orderedRegions);
        for (int i = 0; i < orderedRegions.Count; i++)
        {
            var current = orderedRegions[i];
            var regionSize = GetRegionRelativeSize(i, mapRegions.Count);
            if (!mapGraph.TryGetNode(current.Id, out var currentNode))
            {
                Trace.WriteLine($"[{nameof(FloodFillGeneration)}.{nameof(ConnectMapRegions)}] Unexpected! Region with id {current.Id} was not found in {nameof(mapGraph)}.");
                continue;
            }

            foreach (var connection in currentNode.Connections)
            {
                var destroyCells = connection.Value.BorderingCells
                    .Skip(Global.RNG.Next(connection.Value.BorderingCells.Count))
                    .Take(1);

                foreach (var cell in destroyCells)
                {
                    if (GlyphData.GlyphAppearanceMap.TryGetValue('.', out var appearance))
                    {
                        cell.Terrain = new Terrain(new Glyph((ColoredGlyphAndEffect)appearance.Clone()));
                        //cell.Glyph.Appearance.IsDirty = true;
                        cell.Map.DirtyCells.Add(cell);
                    }
                }
            }
        }

        static RegionSize GetRegionRelativeSize(int i, int count)
        {
            var val = (float)i / count;
            var enumSize = Enum.GetValues<RegionSize>().Length;
            var index = (int)(val * enumSize);

            return (RegionSize)index;
        };
    }

    public static List<RegionCellData> FloodFillMap(GameMap map)
    {
        List<RegionCellData> mapRegions = [];
        Queue<Cell> regionNeighbours = [];
        HashSet<Cell> borderCells = [];
        HashSet<Cell> region = [];
        HashSet<Cell> openCells = GetOpenCells(map);
        Cell current = null!;

        do
        {
            if (regionNeighbours.Count == 0)
            {
                if (region.Count != 0)
                {
                    mapRegions.Add(new RegionCellData(region, borderCells));
                }

                region = [];
                borderCells = [];
                current = openCells.First();
            }
            else
            {
                current = regionNeighbours.Dequeue();
            }

            current.Properties[Props.IsFloodFilled] = true;
            region.Add(current);

            if (!openCells.Remove(current))
            {
                Trace.WriteLine($"Unexpected! Cell at {current.Position} was not found in the {nameof(openCells)}!");
            }

            var neighbourTraversableCells = current.GetValidNeighbours(neighbourCell => !neighbourCell.IsValidForEntry(GObjType.NPC) || neighbourCell.Properties[Props.IsFloodFilled]);

            foreach (var neighbour in neighbourTraversableCells)
            {
                if (!regionNeighbours.Contains(neighbour))
                {
                    regionNeighbours.Enqueue(neighbour);
                }
            }

            var regionBorderCells = current.GetValidNeighbours(neighbourCell => neighbourCell.IsValidForEntry(GObjType.NPC));
            foreach (var border in regionBorderCells)
            {
                borderCells.Add(border);
            }

        } while (openCells.Count > 0);

        return mapRegions;
    }

    internal static MapGraph GenerateMapGraph(List<RegionCellData> mapRegions)
    {
        var mapGraph = new MapGraph();
        foreach (var region in mapRegions)
        {
            if (!mapGraph.TryGetNode(region.Id, out var currentNode))
            {
                currentNode = new MapNode(region);
                mapGraph.AddNode(currentNode);
            }

            foreach (var borderCell in region.Borders)
            {
                var borderingRegions = mapRegions.Where(r => region != r && r.Borders.Contains(borderCell));
                foreach (var neighbourRegion in borderingRegions.ToList())
                {
                    if (!mapGraph.TryGetNode(neighbourRegion.Id, out var neighbourNode))
                    {
                        neighbourNode = new MapNode(neighbourRegion);
                        mapGraph.AddNode(neighbourNode);
                    }

                    if (!currentNode.Connections.TryGetValue(neighbourNode.Region.Id, out var neighbourConnection))
                    {
                        neighbourConnection = new MapConnection(neighbourNode);
                        currentNode.Connections.Add(neighbourNode.Region.Id, neighbourConnection);
                    }

                    neighbourConnection.BorderingCells.Add(borderCell);
                }
            }
        }

        return mapGraph;
    }

    private static HashSet<Cell> GetOpenCells(GameMap map)
    {
        var openPositionsList = new HashSet<Cell>();
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                var currentCell = map[x, y];
                if (map.IsInValidMapBounds(x, y) && currentCell.IsValidForEntry(GObjType.NPC) && !currentCell.Properties[Props.IsFloodFilled])
                {
                    openPositionsList.Add(currentCell);
                }
            }
        }

        return openPositionsList;
    }

    private enum RegionSize
    {
        Tiny = 0,
        Minuscule,
        Small,
        Medium,
        Large,
        Massive
    }
}
