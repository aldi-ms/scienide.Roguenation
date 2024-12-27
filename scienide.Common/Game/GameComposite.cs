namespace scienide.Common.Game;

using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

public abstract class GameComposite(Point pos) : GameComponent, IGameComposite
{
    private readonly Dictionary<GObjType, IGameComponent> _components = [];

    public Point Position { get; set; } = pos;

    public ReadOnlyCollection<IGameComponent> Children => _components.Values.ToList().AsReadOnly();

    public bool AddComponent(IGameComponent component)
    {
        if (component == null)
        {
            return false;
        }

        if (_components.TryGetValue(component.ObjectType, out var found))
        {
            Trace.WriteLine($"[{nameof(GameComposite)}.{nameof(AddComponent)}]: Overwriting child {component.ObjectType} value with {component}.");
            found = component;
        }
        else
        {
            _components.Add(component.ObjectType, component);
        }

        component.Parent = this;
        OnComponentAdded(component);
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

    public bool RemoveComponent(IGameComponent component)
    {
        if (component == null)
        {
            return false;
        }

        if (_components.Remove(component.ObjectType))
        {
            OnComponentRemoved(component);
            component.Parent = null;
            return true;
        }

        return false;
    }

    protected virtual void OnComponentAdded(IGameComponent component) { }
    protected virtual void OnComponentRemoved(IGameComponent component) { }
}
