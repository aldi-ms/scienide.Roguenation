namespace scienide.Common.Game;

using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

public abstract class GameComposite(Point pos) : GameComponent, IGameComposite
{
    private readonly Dictionary<GObjType, IGameComponent> _components = [];

    public Point Position { get; set; } = pos;

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

    public bool TryGetComponent<T>(GObjType gameObjType, out T? component) where T : class, IGameComponent
    {
        foreach (var kvp in _components)
        {
            if ((kvp.Key & gameObjType) != 0)
            {
                component = kvp.Value as T;
                return true;
            }
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
