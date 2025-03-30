namespace scienide.Engine.Game.Pathfinding;

using SadRogue.Primitives;

/// <summary>
/// 
/// This is how you could use A* pathfinding
/// <code>
/// Point goal = Point.None;
/// do
/// {
///     goal = new Point(Global.RNG.Next(_gameMap.Width), Global.RNG.Next(_gameMap.Height));
/// } while (!_gameMap.IsInValidMapBounds(goal.X, goal.Y) || !_gameMap[goal].IsValidCellForEntry(GObjType.Player | GObjType.NPC));
///
/// var path = AStar.AStarSearch(Hero.Position, goal, NeighbourCache.MapNeighbours);
///
/// GameMap.HighlightPath(_gameMap, path);
///
/// if (path.Length == 0)
///    NeighbourCache.DumpNeighbourCache();
/// </code>
/// </summary>
public static class AStar
{
    public static Point[] AStarSearch(Point start, Point goal, Dictionary<Point, Point[]> cellNeighbours)
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
                return TakePath(cameFrom, current);
            }

            foreach (var neighbour in cellNeighbours[current])
            {
                var tentativeCost = costSoFar[current] + GetMoveCost(current, neighbour);

                if (!costSoFar.TryGetValue(neighbour, out int bestCost) || tentativeCost < bestCost)
                {
                    costSoFar[neighbour] = tentativeCost;
                    cameFrom[neighbour] = current;
                    openSet.Enqueue(neighbour, tentativeCost + ManhattanDistance(neighbour, goal));
                }
            }
        }

        return [];
    }

    private static int GetMoveCost(Point from, Point to)
    {
        var delta = from.Subtract(to);
        if (delta.X == 0 || delta.Y == 0)
        {
            return 10;
        }

        return 14;
    }

    public static Point[] TakePath(Dictionary<Point, Point> cameFrom, Point current)
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

    public static float DiagonalDistance(Point x, Point goal)
    {
        var dx = Math.Abs(x.X - goal.X);
        var dy = Math.Abs(x.Y - goal.Y);

        return (dx + dy) + (-0.6f * Math.Min(dx, dy));
    }

    public static float ManhattanDistance(Point a, Point b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}
