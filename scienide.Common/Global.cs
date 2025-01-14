namespace scienide.Common;

using MathNet.Numerics.Random;
using SadConsole.StringParser;
using System;

public static class Global
{
    private static readonly int _seed = RandomSeed.Robust();

    public const bool EnableFov = false;
    public const int MapGenRegionSize = 3;

    /// <summary>
    /// Thread unsafe random number generator, based on <see cref="Xoshiro256StarStar"/>
    /// </summary>
    public static readonly Random RNG = new Xoshiro256StarStar(_seed, false);
    public static readonly IParser StringParser = new Default();
    public static readonly Ulid NoneActionId = Ulid.NewUlid();
    public static readonly Ulid HeroId = Ulid.NewUlid();
    public static readonly Ulid TimeSentinelId = Ulid.NewUlid();

    public static int Seed => _seed;
}
