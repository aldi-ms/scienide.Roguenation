using SadRogue.Primitives;
using scienide.Engine.Core;

namespace scienide.Engine;

public static class Global
{
    public static Random RNG = new();

    public static Direction GetRandomValidDirection()
    {
        int dX, dY;
        do
        {
            dX = Global.RNG.Next(-1, 2);
            dY = Global.RNG.Next(-1, 2);
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
