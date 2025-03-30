namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

public abstract class GameComposite : GameComponent, IGameComposite
{
    private readonly List<IGameComponent> _components = [];

    public ReadOnlyCollection<IGameComponent> Components => _components.AsReadOnly();

    protected virtual void OnComponentAdded(IGameComponent component) { }
    protected virtual void OnComponentRemoved(IGameComponent component) { }

    public bool AddComponent<T>(T component) where T : IGameComponent
    {
        if (component == null)
        {
            Trace.WriteLine("Attempt to add null component!");
            return false;
        }

        if (_components.IndexOf(component) != -1)
        {
            Trace.WriteLine("Attempt to add component that already exists!");
            return false;
        }

        _components.Add(component);
        component.Parent = this;
        OnComponentAdded(component);

        return true;
    }

    public bool TryGetComponents<T>([NotNullWhen(true)] out IEnumerable<T>? components)
    {
        components = _components.OfType<T>();
        return components.Any();
    }

    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component, bool searchRecursive = false)
    {
        var foundComponents = _components.OfType<T>();
        ArgumentOutOfRangeException.ThrowIfGreaterThan(foundComponents.Count(), 1, nameof(foundComponents));
        component = foundComponents.FirstOrDefault();

        if (component == null && searchRecursive)
        {
            var compositeComponents = _components.OfType<IGameComposite>();
            foreach (var composite in compositeComponents)
            {
                if (composite.TryGetComponent<T>(out var foundNestedComponent, searchRecursive))
                {
                    component = foundNestedComponent;
                    return true;
                }
            }
        }

        return component != null;
    }

    public bool RemoveComponent<T>(T component) where T : IGameComponent
    {
        if (component == null)
        {
            Trace.WriteLine("Attempt to remove null component!");
            return false;
        }

        if (_components.Remove(component))
        {
            OnComponentRemoved(component);
            component.Parent = null;

            return true;
        }

        return false;
    }

    public bool RemoveComponents<T>() where T : IGameComponent
    {
        var result = false;
        if (TryGetComponents<T>(out var components))
        {
            var list = components.ToArray();
            for (var i = 0; i < list.Length; i++)
            {
                if (RemoveComponent(list[i]))
                {
                    result = true;
                }
            }
        }

        return result;
    }
}
