namespace scienide.Engine.Core;

[Flags]
public enum CollisionLayer : uint
{
    None        = 0,
    Map         = 1 << 0,
    Terrain     = 1 << 1,
    Projectiles = 1 << 2,
    Actor       = 1 << 3
}


internal class Collision
{

}
