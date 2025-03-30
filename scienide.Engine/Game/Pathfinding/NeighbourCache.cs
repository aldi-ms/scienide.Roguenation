namespace scienide.Engine.Game.Pathfinding;

using Newtonsoft.Json;
using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Infrastructure;

public static class NeighbourCache
{
    private static readonly Dictionary<Point, Point[]> _mapNeighbours = [];

    public static Dictionary<Point, Point[]> MapNeighbours => _mapNeighbours;

    public static void InitMapNeighbours(FlatArray<Cell> map)
    {
        var neighborCells = new Point[8];
        foreach (var cell in map)
        {
            if (cell.Properties[Props.IsWalkable])
            {
                var neighbours = GetValidWalkableNeighbours(cell, map, neighborCells);
                _mapNeighbours.Add(cell.Position, neighbours);
            }
        }
    }

    public static void DumpNeighbourCache()
    {
        var json = JsonConvert.SerializeObject(_mapNeighbours, Formatting.Indented);
        File.WriteAllText(@".\PathfindingNeighbour.dump.txt", json);
    }

    private static Point[] GetValidWalkableNeighbours(Cell cell, FlatArray<Cell> map, Point[] neighborArr)
    {
        int neighborCount = 0;
        for (int dX = -1; dX <= 1; dX++)
        {
            for (int dY = -1; dY <= 1; dY++)
            {
                if (dX == 0 && dY == 0)
                    continue;

                var x = cell.Position.X + dX;
                var y = cell.Position.Y + dY;

                if (x < 0 || x >= map.Width || y < 0 || y >= map.Height)
                {
                    continue;
                }

                if (!map[x, y].Properties[Props.IsWalkable])
                {
                    continue;
                }
                neighborArr[neighborCount++] = new Point(x, y);
            }
        }

        var resultArray = new Point[neighborCount];
        Array.Copy(neighborArr, resultArray, neighborCount);
        return resultArray;
    }
}
