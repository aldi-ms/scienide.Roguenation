namespace scienide.Engine.Infrastructure;

using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game;

/// <summary>
/// A doubly-linked circular list with travelling sentinel,
/// implemented for the game time system.
/// </summary>
public class TimeManager
{
    private readonly Node _sentinel;
    private Node _current;

    public TimeManager()
    {
        _sentinel = new Node(new SentinelTimeEntity());
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
            var action = _current.Entity.TakeTurn();
            var cost = action.Execute();
            _current.Entity.Energy -= cost;
            if (_current.Entity.Actor != null)
            {
                _current.Entity.Actor.Action = null;
            }
        }
    }

    public void ProgressSentinel()
    {
        // Check if only a single entity is added
        if (_sentinel.Next == _sentinel.Prev)
        {
            return;
        }

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

    public void Add(ITimeEntity item)
    {
        if (item != null)
        {
            var node = new Node(item);
            node.Next = _sentinel;
            node.Prev = _sentinel.Prev;

            _sentinel.Prev.Next = node;
            _sentinel.Prev = node;
        }
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
    public class Node(ITimeEntity data)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();
        public ITimeEntity Entity { get; set; } = data;
        public Node Next { get; set; }
        public Node Prev { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }

    private class SentinelTimeEntity : TimeEntity
    {
        public SentinelTimeEntity() : base(0, 1)
        {
        }

        public override IActionCommand TakeTurn()
        {
            throw new InvalidOperationException($"{nameof(SentinelTimeEntity)}.{nameof(TakeTurn)} should not be called!");
        }
    }
}
