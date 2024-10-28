namespace scienide.Common;

using SadRogue.Primitives;

public static class Global
{
    private static readonly int _seed = (int)DateTime.UtcNow.Ticks;

    public static readonly Random RNG = new(_seed);

    public static int Seed => _seed;
    public static readonly Ulid NoneActionId = Ulid.NewUlid();
    public static readonly Ulid HeroId = Ulid.NewUlid();

    public static Direction GetRandomValidDirection()
    {
        int dX, dY;
        do
        {
            dX = RNG.Next(-1, 2);
            dY = RNG.Next(-1, 2);
        }
        while (dX == 0 && dY == 0);

        return Direction.GetCardinalDirection(dX, dY);
    }

    public static double CalculateManhattanDistance(Point point1, Point point2)
    {
        return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
    }
}
