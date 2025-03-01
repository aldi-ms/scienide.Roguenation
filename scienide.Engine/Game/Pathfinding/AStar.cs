namespace scienide.Engine.Game.Pathfinding;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Infrastructure;

public static class AStar
{
    public static Point[] AStarSearch(FlatArray<Cell> map, Point start, Point goal)
    {
        var openSet = new PriorityQueue<Point, float>();
        openSet.Enqueue(start, 0);

        var cameFrom = new Dictionary<Point, Point>();
        var costSoFar = new Dictionary<Point, int>
        {
            { start, 0 }
        };

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current.Equals(goal))
            {
                return GeneratePath(cameFrom, current);
            }

            foreach (var neighbour in NeighbourCache.MapNeighbours[current])
            {
                var tentativeCost = costSoFar[current] + GetMoveCost(current, neighbour);

                if (!costSoFar.TryGetValue(neighbour, out int bestCost) || tentativeCost < bestCost)
                {
                    costSoFar[neighbour] = tentativeCost;
                    cameFrom[neighbour] = current;
                    openSet.Enqueue(neighbour, Heuristic(neighbour, goal));
                }
            }
        }

        return [];
    }

    private static int GetMoveCost(Point from, Point to)
    {
        if (!NeighbourCache.MapNeighbours[from].Contains(to))
        {
            // should just be used for neighbour cells
            throw new ArgumentException($"{nameof(GetMoveCost)} should be used for neighbouring points.");
        }

        var delta = from.Subtract(to);
        if (delta.X == 0 || delta.Y == 0)
        {
            return 10;
        }

        return 14;
    }

    public static Point[] GeneratePath(Dictionary<Point, Point> cameFrom, Point current)
    {
        List<Point> path = [current];

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        return path.ToArray();
    }

    public static float Heuristic(Point x, Point goal)
    {
        var dx = Math.Abs(x.X - goal.X);
        var dy = Math.Abs(x.Y - goal.Y);
        return  (dx + dy) + (-0.6f * Math.Min(dx, dy));
    }
}
