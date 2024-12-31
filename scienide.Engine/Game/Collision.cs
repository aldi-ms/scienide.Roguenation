namespace scienide.Engine.Game;

using scienide.Common.Game;

public class Collision
{
    /// <summary>
    /// Which layer collides with which layers
    /// </summary>
    public static Dictionary<Layer, Layer[]> Collisions = new()
    {
        { Layer.None, [Layer.None] },
        { Layer.Projectiles, [Layer.Actor] },
        { Layer.Actor, [Layer.Projectiles | Layer.Actor] }
    };
}
