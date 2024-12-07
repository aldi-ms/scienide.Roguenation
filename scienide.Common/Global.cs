namespace scienide.Common;

using MathNet.Numerics.Random;
using SadConsole.StringParser;
using SadRogue.Primitives;
using System;

public static class Global
{
    public const bool EnableFov = true;
    public const int MapGenRegionSize = 3;

    private static readonly int _seed = RandomSeed.Robust();

    /// <summary>
    /// Thread unsafe random number generator, based on <see cref="Xoshiro256StarStar"/>
    /// </summary>
    public static readonly Random RNG = new Xoshiro256StarStar(_seed, false);

    public static readonly IParser StringParser = new Default();

    public static readonly Ulid NoneActionId = Ulid.NewUlid();
    public static readonly Ulid HeroId = Ulid.NewUlid();
    public static readonly Ulid TimeSentinelId = Ulid.NewUlid();

    public static int Seed => _seed;

    public static readonly Point[] DeltaCardinalNeighborDir =
    [
        new(-1, 0),
        new(0, -1),
        new(1, 0),
        new(0, 1),
    ];

    public static Direction GetRandomValidDirection()
    {
        int dX, dY;
        do
        {
            dX = RNG.Next(-1, 2);
            dY = RNG.Next(-1, 2);
        }
        while (dX == 0 && dY == 0);

        return Direction.GetDirection(dX, dY);
    }

    public static float ManhattanDistance(Point point1, Point point2)
    {
        return MathF.Abs(point1.X - point2.X) + MathF.Abs(point1.Y - point2.Y);
    }

    public static float PythagoreanDistance(Point point1, Point point2)
    {
        return MathF.Sqrt(((point1.X - point2.X) * (point1.X - point2.X)) + ((point1.Y - point2.Y) * (point1.Y - point2.Y)));
    }
}
