namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;

public abstract class GameComponent : IGameComponent
{
    public IGameComponent? Parent { get; set; }
    public GObjType ObjectType { get; set; }
    public virtual string Status { get; } = null!;
}
