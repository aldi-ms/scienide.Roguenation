namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;
using System.Diagnostics;

public abstract class GameComponent : IGameComponent
{
    public IGameComponent? Parent { get; set; }
    public Glyph Glyph { get; set; }
    public CollisionLayer Layer { get; set; } = CollisionLayer.None;
    public GObjType ObjectType { get; set; }
}
