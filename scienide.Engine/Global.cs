﻿namespace scienide.Engine;

using SadRogue.Primitives;
using scienide.Engine.Core;

public static class Global
{
    private static readonly int _seed = (int)DateTime.UtcNow.Ticks;
    public static readonly Random RNG = new(_seed);

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
        while (false);

        return Direction.GetCardinalDirection(dX, dY);
    }

    /// <summary>
    /// Which layer collides with which layers
    /// </summary>
    public static Dictionary<CollisionLayer, CollisionLayer[]> Collisions = new()
    {
        { CollisionLayer.None, [CollisionLayer.None] },
        { CollisionLayer.Projectiles, [CollisionLayer.Actor] },
        { CollisionLayer.Actor, [CollisionLayer.Projectiles | CollisionLayer.Actor] }
    };
}
