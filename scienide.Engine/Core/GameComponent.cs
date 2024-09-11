using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Core;

public abstract class GameComponent : IGameComponent
{
    public IGameComponent? Parent { get; set; }
}
