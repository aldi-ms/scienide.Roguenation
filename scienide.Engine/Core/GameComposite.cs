using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using System.Collections.ObjectModel;

namespace scienide.Engine.Core;

public abstract class GameComposite : GameComponent, IGameComposite
{
    private readonly List<IGameComponent> _children;
    private readonly ReadOnlyCollection<IGameComponent> _readonlyChildren;

    public GameComposite(Point pos) : base(pos)
    {
        _children = [];
        _readonlyChildren = _children.AsReadOnly();
    }

    public ReadOnlyCollection<IGameComponent> Children => _readonlyChildren;

    public bool AddChild(IGameComponent child)
    {
        if (child == null || _children.Contains(child))
        {
            return false;
        }

        child.Parent = this;
        _children.Add(child);
        return true;
    }

    public bool RemoveChild(IGameComponent child)
    {
        if (child == null)
        {
            return false;
        }

        child.Parent = null;
        return _children.Remove(child);
    }

    public override void Traverse(Action<IGameComponent> action)
    {
        base.Traverse(action);
        foreach (var child in _children)
        {
            child.Traverse(action);
        }
    }
}
