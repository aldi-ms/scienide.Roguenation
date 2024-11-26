namespace scienide.Engine.Map;

using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Engine.Game;
using System.Diagnostics;

public class FloodFillGeneration
{
    private readonly List<Cell> _openCellPositions = [];

    public FloodFillGeneration(GameMap map)
    {
        _openCellPositions = GetAllUnfilledPositions(map);
    }

    public void FloodFillAndConnect()
    {
        var separateCellRegions = new List<List<Cell>>();

        do
        {
            var region = new List<Cell>();
            FloodFillRecursive(_openCellPositions.First(), ref region);
            separateCellRegions.Add(region);
        } while (_openCellPositions.Count > 0);
    }

    public void FloodFillRecursive(Cell current, ref List<Cell> region)
    {
        // Flood fill the current cell
        current.Properties[Props.IsFloodFilled] = true;
        region.Add(current);

        if (!_openCellPositions.Remove(current))
        {
            Trace.WriteLine($"Unexpected! Processed cell was not found in the {nameof(_openCellPositions)}!");
            return;
        }

        // Get cell's neighbours that are not yet flood-filled
        var neighbours = current.GetValidNeighbors(x => x.Properties[Props.IsFloodFilled]);

        for (var i = 0; i < neighbours.Length; i++)
        {
            // Call recursively for all neighbours
            FloodFillRecursive(neighbours[i], ref region);
        }
    }

    private static List<Cell> GetAllUnfilledPositions(GameMap map)
    {
        var openPositionsList = new List<Cell>();
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                var currentCell = map[x, y];
                if (map.IsInValidMapBounds(x, y) && currentCell.IsValidForEntry(GObjType.CodeEntry) && !currentCell.Properties[Props.IsFloodFilled])
                {
                    openPositionsList.Add(currentCell);
                }
            }
        }

        return openPositionsList;
    }
}
