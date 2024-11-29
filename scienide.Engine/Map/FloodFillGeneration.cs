namespace scienide.Engine.Map;

using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Engine.Game;
using System.Diagnostics;

public class FloodFillGeneration
{
    public static List<List<Cell>> FloodFillMap(GameMap map)
    {
        List<List<Cell>> mapRegions = [];
        Queue<Cell> regionNeighbours = [];
        List<Cell> region = [];
        HashSet<Cell> openCells = GetOpenCells(map);
        Cell current = null!;

        do
        {
            if (regionNeighbours.Count == 0)
            {
                if (region.Count != 0)
                {
                    mapRegions.Add(region);
                }

                region = [];
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

            var neighbourCells = current.GetValidNeighbours(neighbourCell => !neighbourCell.IsValidForEntry(GObjType.NPC) || neighbourCell.Properties[Props.IsFloodFilled]);
            foreach (var neighbour in neighbourCells)
            {
                if (!regionNeighbours.Contains(neighbour))
                {
                    regionNeighbours.Enqueue(neighbour);
                }
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
}
