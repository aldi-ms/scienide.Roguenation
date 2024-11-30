namespace scienide.Engine.Map;

using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Engine.Game;
using System.Diagnostics;
using System.Net;

public class FloodFillGeneration
{
    /// <summary>
    /// Make sure all regions are reachable by w/e means possible.
    /// Bigger regions should have at least 2 entries - single/double doors.
    /// Smaller regions should have at least 1 single door entry.
    /// Smallest can have a (hidden) teleport leading inside of them.
    /// </summary>
    /// <param name="mapRegions"></param>
    public static void ConnectMapRegions(List<List<Cell>> mapRegions)
    {
        var orderedRegions = mapRegions.OrderBy(x => x.Count).ToList();

        for (int i = 0; i < orderedRegions.Count; i++)
        {
            var current = orderedRegions[i];
            var regionSize = GetRegionRelativeSize(i, mapRegions.Count);
            //regionSize switch
            //{
            //    RegionSize.Tiny => ,

            //}
        }

        static RegionSize GetRegionRelativeSize(int i, int count)
        {
            var val = (float)i / count;
            var enumSize = Enum.GetValues<RegionSize>().Length;
            var index = (int)(val * enumSize);

            return (RegionSize)index;
        };
    }

    public static List<RegionData> FloodFillMap(GameMap map)
    {
        List<RegionData> mapRegions = [];
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
                    mapRegions.Add(new RegionData(region, borderCells));
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

    public class RegionData(HashSet<Cell> cells, HashSet<Cell> walls)
    {
        public HashSet<Cell> Cells { get; set; } = cells;
        public HashSet<Cell> Walls { get; set; } = walls;
    }

    private enum RegionSize
    {
        Tiny = 0,
        Small,
        Medium,
        Large,
        Massive,
        Immense
    }
}
