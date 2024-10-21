namespace scienide.Engine.Core;

using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game;
using System.Diagnostics;

public abstract class GameComponent : IGameComponent
{
    public IGameComponent? Parent { get; set; }
    public Glyph Glyph { get; set; }
    public CollisionLayer Layer { get; set; } = CollisionLayer.None;
    public GameObjType ObjectType { get; set; }

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
