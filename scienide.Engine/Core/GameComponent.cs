using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game;
using System.Diagnostics;

namespace scienide.Engine.Core;

public abstract class GameComponent : IGameComponent
{
    public IGameComponent? Parent { get; set; }
    public Glyph? Glyph { get; set; }
    public Point Position { get; set; }
    
    public GameComponent(Point position)
    {
        Position = position;
    }

    public virtual void Traverse(Action<IGameComponent> action)
    {
        try
        {
            action(this);
        }
        catch (Exception ex)
        {
            // We shouldn't crash here
            Trace.WriteLine($"{nameof(GameComponent)}.{nameof(Traverse)} " + ex);
        }
    }
}
