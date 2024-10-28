namespace scienide.Engine.Core;

using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Infrastructure;
using System.Collections.ObjectModel;
using System.Diagnostics;

public abstract class GameComposite : GameComponent, IGameComposite
{
    private readonly Dictionary<GObjType, IGameComponent> _components;

    public GameComposite(Point pos)
    {
        _components = [];
        Position = pos;
    }

    public Point Position { get; set; }

    // Do we need that readonly?
    public ReadOnlyCollection<IGameComponent> Children => _components.Values.ToList().AsReadOnly();

    public bool AddChild(IGameComponent child)
    {
        if (child == null)
        {
            return false;
        }

        if (_components.TryGetValue(child.ObjectType, out var component))
        {
            Trace.WriteLine($"[{nameof(GameComposite)}.{nameof(AddChild)}]: Overwriting child {child.ObjectType} value with {child}.");
            component = child;
        }
        else
        {
            _components.Add(child.ObjectType, child);
        }

        child.Parent = this;
        return true;
    }

    public bool GetComponent<T>(GObjType gameObjType, out T? component) where T : class, IGameComponent
    {
        if (_components.TryGetValue(gameObjType, out var c))
        {
            component = c as T;
            return true;
        }

        component = null;
        return false;
    }

    public bool RemoveChild(IGameComponent child)
    {
        if (child == null)
        {
            return false;
        }

        child.Parent = null;
        return _components.Remove(child.ObjectType);
    }
}
