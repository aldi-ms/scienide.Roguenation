namespace scienide.Engine.Map;

using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Engine.Game;
using System.Diagnostics;

public class FloodFillGeneration
{
    private readonly HashSet<Cell> _openCellPositions = [];
    private readonly HashSet<Cell> _closedCellPositions = [];

    public FloodFillGeneration(GameMap map)
    {
        _openCellPositions = GetAllUnfilledPositions(map);
    }

    public List<List<Cell>> FloodFillAndConnect()
    {
        var separateCellRegions = new List<List<Cell>>();

        do
        {
            var region = new List<Cell>();
            FloodFillRecursive(_openCellPositions.First(), ref region);
            separateCellRegions.Add(region);
        } while (_openCellPositions.Count > 0);

        return separateCellRegions;
    }

    public void FloodFillRecursive(Cell current, ref List<Cell> region)
    {
        // Flood fill the current cell
        current.Properties[Props.IsFloodFilled] = true;
        region.Add(current);

        if (!_openCellPositions.Remove(current))
        {
            Trace.WriteLine($"Unexpected! Open cell was not found in the {nameof(_openCellPositions)}!");
            return;
        }

        if (!_closedCellPositions.Add(current))
        {
            Trace.WriteLine($"Unexpected! Closed cell was not found in the {nameof(_openCellPositions)}!");
            return;
        }

        // Get cell's neighbours that are not yet flood-filled
        var neighbours = current.GetValidNeighbors(x => x.Properties[Props.IsFloodFilled] || _closedCellPositions.Contains(x));

        for (var i = 0; i < neighbours.Length; i++)
        {
            if (_closedCellPositions.Contains(neighbours[i]))
            {
                // TODO: cant understand why we end up here??
                continue;
            }

            // Call recursively for all neighbours
            FloodFillRecursive(neighbours[i], ref region);
        }
    }

    private static HashSet<Cell> GetAllUnfilledPositions(GameMap map)
    {
        var openPositionsList = new HashSet<Cell>();
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
