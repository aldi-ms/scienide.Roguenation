namespace scienide.Common.Game;

using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

public abstract class GameComposite(Point pos) : GameComponent, IGameComposite
{
    private readonly List<IGameComponent> _components = [];

    public Point Position { get; set; } = pos;

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

        _components.Add(component);

        component.Parent = this;
        OnComponentAdded(component);

        return true;
    }

    public bool TryGetComponents<T>([NotNullWhen(true)] out IEnumerable<T>? components) where T : IGameComponent
    {
        components = _components.OfType<T>();
        return components != null && components.Any();
    }

    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component) where T : IGameComponent
    {
        var foundComponents = _components.OfType<T>();
        ArgumentOutOfRangeException.ThrowIfGreaterThan(foundComponents.Count(), 1, nameof(foundComponents));
        component = foundComponents.FirstOrDefault();
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
