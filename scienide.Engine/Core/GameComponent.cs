using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Core;

public abstract class GameComponent : IGameComponent
{
    public IGameComponent? Parent { get; set; }

    public char Glyph { get; set; }

    public virtual void Traverse(Action<IGameComponent> action)
    {
        action(this);
    }
}
