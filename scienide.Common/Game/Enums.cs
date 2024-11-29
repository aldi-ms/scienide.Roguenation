namespace scienide.Common.Game;

[Flags]
public enum CollisionLayer : uint
{
    None        = 0,
    Map         = 1 << 0,
    Terrain     = 1 << 1,
    Projectiles = 1 << 2,
    Actor       = 1 << 3
}

[Flags]
public enum GObjType : uint
{
    None        = 0,
    Map         = 1 << 0,
    Terrain     = 1 << 1,
    Projectiles = 1 << 2,
    Player      = 1 << 3,
    NPC         = 1 << 4,

    Any         = 1 << 30
}