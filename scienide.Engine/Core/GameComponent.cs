using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game;

namespace scienide.Engine.Core;

public abstract class GameComponent : IGameComponent
{
    public IGameComponent? Parent { get; set; }

    public Glyph Glyph { get; set; }

    protected GameComponent()
    {
        Glyph = new Glyph();
    }

    public virtual void Traverse(Action<IGameComponent> action)
    {
        action(this);
    }
}
