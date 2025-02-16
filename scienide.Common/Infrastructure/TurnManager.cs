namespace scienide.Common.Infrastructure;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

public class TurnManager
{
    private readonly LinkedList<ITimeEntity> _entities = [];
    private bool _gainEnergy = true;
    private ulong _gameTicks;
    private LinkedListNode<ITimeEntity>? _currentNode;
    //private Memory<TimeEntity> _memoryEntities;

    public TurnManager()
    {
       // spanEntities.ad
    }

    public ulong GameTicks => _gameTicks;

    public void ProcessNext()
    {
        if (_entities.Count == 0)
        {
            _gainEnergy = false;
            return;
        }

        var entity = _currentNode!.Value;
        ArgumentNullException.ThrowIfNull(entity.Actor);

        if (_gainEnergy)
        {
            entity.Energy += entity.Speed;
        }

        if (entity.Energy >= 0)
        {
            var action = entity.TakeTurn();

            if (entity.Actor.TypeId == Global.HeroId)
            {
                if (action.Id == Global.NoneActionId)
                {
                    _gainEnergy = false;
                    return;
                }
            }

            _gainEnergy = true;
            _gameTicks += 1;

            ActionResult result;
            do
            {
                result = action.Execute();

                if (result.Succeeded)
                {
                    if (result.Finished)
                    {
                        entity.Energy -= result.Cost;
                    }
                    else if (result.AlternativeAction != null)
                    {
                        action = result.AlternativeAction;
                    }
                }
                else if (!result.Finished && result.ContinueWith != null)
                {
                    throw new NotImplementedException();
                }
            } while (!result.Finished);

            entity.Actor.ClearAction();
        }

        _currentNode = _currentNode.Next ?? _entities.First;
    }

    public void AddEntity(ITimeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        //_memoryEntities.Clear();

        var node = _entities.AddLast(entity);
        _currentNode ??= node;
    }

    public bool RemoveNode(LinkedListNode<ITimeEntity> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node == _currentNode)
        {
            _currentNode = _currentNode.Next ?? _entities.First;
        }

        _entities.Remove(node);

        return true;
    }

    public bool RemoveEntity(ITimeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var node = _entities.Find(entity);
        if (node == null) return false;

        if (_currentNode == node)
        {
            _currentNode = node.Next ?? _entities.First;
        }

        _entities.Remove(node);
        if (_entities.Count == 0)
        {
            _currentNode = null;
        }

        return true;
    }
}
