using SadRogue.Primitives;
using scienide.Engine.Game.Pathfinding;

namespace scienide.Engine.UnitTests;

public class AStarTests
{
    [Fact]
    public void TestDiagonalHeuristicIsMoreExpensive()
    {
        var start = Point.Zero;
        var goal = new Point(1, 1);

        var diagonalDistance = AStar.Heuristic(start, goal);

        goal = new Point(1, 0);
        var cardinalDistance = AStar.Heuristic(start, goal);

        Assert.True(diagonalDistance > cardinalDistance);
    }

    [Fact]
    public void TestCardinalHeuristicsAreEqual()
    {
        var start = Point.Zero;
        var goal = new Point(10, 0);

        var xDistance = AStar.Heuristic(start, goal);
        goal = new Point(0, 10);

        var yDistance = AStar.Heuristic(start, goal);

        Assert.True(xDistance == yDistance);
    }

    [Fact]
    public void TestNeighbourHeuristic()
    {
        var goal = new Point(15, 7);
        var a = new Point(8, 15);
        var b = new Point(9, 15);

        var ha = AStar.Heuristic(a, goal);
        var hb = AStar.Heuristic(b, goal);

        Assert.True(hb < ha);
    }
} 
