namespace scienide.Engine.Core;

[Flags]
public enum GObjType : uint
{
    None                  = 0,
    Map                   = 1 << 0,
    Terrain               = 1 << 1,
    Projectiles           = 1 << 2,
    ActorPlayerControl    = 1 << 3,
    ActorNonPlayerControl = 1 << 4,
}
