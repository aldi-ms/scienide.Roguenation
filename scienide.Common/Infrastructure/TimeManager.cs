namespace scienide.Common.Infrastructure;

using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// A doubly-linked circular list with travelling sentinel,
/// implemented for the game time system.
/// </summary>
public class TimeManager : IEnumerable<IActor>
{
    private readonly Node _sentinel;

    private bool _gainEnergy;
    private ulong _gameTicks;
    private Node _current;

    public TimeManager()
    {
        //_entities = [];
        _sentinel = new Node(new SentinelTimeEntity());
        _sentinel.Next = _sentinel;
        _sentinel.Prev = _sentinel;
        _current = _sentinel;
        _gameTicks = 0;
        _gainEnergy = true;
    }

    public ulong GameTicks => _gameTicks;

    public int ActorCount { get; private set; }

    /// <summary>
    /// Progress to the next actor in line and if it has enough energy, TakeTurn on it.
    /// </summary>
    /// <returns><c>True</c> if we should wait for input, <c>false</c> otherwise. <c>True</c> means we need to stop calling <see cref="ProgressSentinel"/>
    /// as well as the gameTicks stop incrementing.</returns>
    public bool ProgressTime()
    {
        if (_sentinel.Next == _sentinel)
        {
            return false;
        }

        _current = _sentinel.Next;
        if (_gainEnergy)
        {
            _current.Entity.Energy += _current.Entity.Speed;
        }

        if (_current.Entity.Energy >= 0)
        {
            var action = _current.Entity.TakeTurn();

            if (_current.Entity.Actor?.TypeId == Global.HeroId)
            {
                if (action.Id == Global.NoneActionId)
                {
                    _gainEnergy = false;
                    return true;
                }
            }

            _gainEnergy = true;
            _gameTicks += 1;

            var cost = action.Execute();
            _current.Entity.Energy -= cost;

            if (_current.Entity.Actor != null)
            {
                _current.Entity.Actor.Action = null;
            }
        }

        return !_gainEnergy;
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
            ActorCount++;
            //_entities.Add(item);
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
        ActorCount--;
    }

    #region Hashset turn implementation
    //// Currently the doubly linked list shows as very slightly faster, so  
    /// this HashSet implementation is commented
    //private HashSet<ITimeEntity> _entities;
    //public bool NewRunActors()
    //{
    //    //for (int i = 0; i < _entities.Count; i++)
    //    foreach (var current in _entities)
    //    {
    //        if (_gainEnergy)
    //        {
    //            current.Energy += current.Speed;
    //        }

    //        if (current.Energy >= 0)
    //        {
    //            var action = current.TakeTurn();

    //            if (current.Actor?.TypeId == Global.HeroId)
    //            {
    //                if (action.Id == Global.NoneActionId)
    //                {
    //                    _gainEnergy = false;
    //                    return true;
    //                }

    //                _timer.Stop();
    //                _counter++;
    //                _elapsedTime += _timer.ElapsedTicks;
    //                if (_counter >= 100)
    //                {
    //                    MessageBroker.Instance.Broadcast(new SystemMessageEventArgs($"100 turns median time: {_elapsedTime / 100d} ticks."));
    //                    _counter = 0;
    //                    _elapsedTime = 0;
    //                }

    //                _timer.Restart();
    //            }

    //            _gainEnergy = true;
    //            _gameTicks += 1;

    //            var cost = action.Execute();
    //            current.Energy -= cost;

    //            if (current.Actor != null)
    //            {
    //                current.Actor.Action = null;
    //            }
    //        }
    //    }

    //    return !_gainEnergy;
    //}
    #endregion

    public Enumerator GetEnumerator() => new(this);

    IEnumerator<IActor> IEnumerable<IActor>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<IActor>
    {
        private readonly TimeManager _timeManager;
        private int _index;

        public Enumerator(TimeManager timeManager)
        {
            _timeManager = timeManager;
            _index = -1;
        }

        public readonly IActor Current => _timeManager._current.Entity.Actor ?? throw new ArgumentNullException(nameof(Current));

        readonly object IEnumerator.Current => Current ?? throw new ArgumentNullException(nameof(Current));

        public bool MoveNext()
        {
            _index++;
            _timeManager.ProgressSentinel();
            _timeManager._current = _timeManager._sentinel.Next;

            return _index < _timeManager.ActorCount;
        }

        public void Reset()
        {
            _index = -1;
        }

        public void Dispose()
        {
        }
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    public class Node(ITimeEntity data)
    {
        public ITimeEntity Entity { get; set; } = data;
        public Node Next { get; set; }
        public Node Prev { get; set; }
#pragma warning restore CS8618
    }

    private class SentinelTimeEntity : TimeEntity
    {
        public SentinelTimeEntity() : base(0, 1)
        {
        }

        public override Ulid Id => Global.TimeSentinelId;

        public override IActionCommand TakeTurn()
        {
            throw new InvalidOperationException($"{nameof(SentinelTimeEntity)}.{nameof(TakeTurn)} should never be called!");
        }
    }
}
