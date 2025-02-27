namespace scienide.Engine.Game.Pathfinding;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Infrastructure;

public static class NeighbourCache
{
    private static readonly Dictionary<Point, Point[]> _mapNeighbours = [];

    public static void InitMapNeighbours(FlatArray<Cell> map)
    {
        var neighborCells = new Point[8];
        foreach (var cell in map)
        {
            if (cell.IsValidCellForEntry(GObjType.NPC | GObjType.Player))
            {
                var neighbours = GetValidWalkableNeighbours(cell, map, neighborCells);
                _mapNeighbours.Add(cell.Position, neighbours);
            }
        }
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

                if (x < 0 || x >= map.Width || y < 0 || y >= map.Height || cell.IsValidCellForEntry(GObjType.Player | GObjType.NPC))
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
