using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Infrastructure;

/// <summary>
/// A doubly-linked circular list with travelling sentinel, for the game time system
/// </summary>
/// <typeparam name="T"></typeparam>
public class CircularList<T> where T : ITimedEntity
{
    private Node _sentinel;
    private Node _current;

    public CircularList()
    {
#pragma warning disable CS8604 // Possible null reference argument.
        _sentinel = new Node(default);
#pragma warning restore CS8604 // Possible null reference argument.

        _sentinel.Next = _sentinel;
        _sentinel.Prev = _sentinel;
        _current = _sentinel;
    }

    public void ProgressTime()
    {
        if (_sentinel.Next == _sentinel)
        {
            return;
        }

        _current = _sentinel.Next;
        _current.Entity.Energy += _current.Entity.Speed;
        if (_current.Entity.Energy >= 0)
        {
            // Take turn
            // subtract turn energy from entity.Energy
        }
    }

    public void ProgressSentinel()
    {
        var x = _sentinel.Prev;
        var a = _sentinel.Next;
        var b = a.Next;

        a.Prev = x;
        x.Next = a;
        _sentinel.Prev = a;
        _sentinel.Next = b;
        a.Next = _sentinel;
        b.Prev = _sentinel;
    }

    public void Insert(T item)
    {
        var node = new Node(item);

        node.Next = _sentinel;
        node.Prev = _sentinel.Prev;
        _sentinel.Prev.Next = node;
        _sentinel.Prev = node;
    }

    public void Remove(Node node)
    {
        if (node == _sentinel)
        {
            return;
        }

        node.Prev.Next = node.Next;
        node.Next.Prev = node.Prev;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class Node(T data)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        public T Entity { get; set; } = data;
        public Node Next { get; set; }
        public Node Prev { get; set; }
    }
}
