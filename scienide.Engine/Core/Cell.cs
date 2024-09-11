using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using System.Collections.ObjectModel;

namespace scienide.Engine.Core;

public class Cell : GameComponent, ICell
{
    private readonly List<IGameComponent> _children;
    private readonly ReadOnlyCollection<IGameComponent> _readonlyChildren;

    public Cell()
    {
        _children = [];
        _readonlyChildren = _children.AsReadOnly();
    }

    public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
}
