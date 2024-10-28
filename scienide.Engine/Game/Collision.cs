namespace scienide.Engine.Game;

using scienide.Engine.Infrastructure;

public class Collision
{
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
