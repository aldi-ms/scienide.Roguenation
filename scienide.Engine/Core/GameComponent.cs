using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game;

namespace scienide.Engine.Core;

public abstract class GameComponent : IGameComponent
{
    public IGameComponent? Parent { get; set; }
    public Glyph? Glyph { get; set; }

    public virtual void Traverse(Action<IGameComponent> action)
    {
        try
        {
            action(this);
        }
        catch (Exception ex)
        {
            // Shouldn't crash the app
            Trace.WriteLine($"{nameof(GameComponent)}.{nameof(Traverse)} " + ex);
        }
    }
}
