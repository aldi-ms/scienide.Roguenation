namespace scienide.Common.Infrastructure;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;

public class TurnManager
{
    private bool _gainEnergy = true;
    private ulong _gameTicks;
    private readonly LinkedList<ITimeEntity> _entities;
    private LinkedListNode<ITimeEntity>? _currentNode;

    public TurnManager()
    {
        MessageBroker.Instance.Subscribe<ActorDeathMessage>(HandleActorDeath);
        _entities = [];
    }

    private void HandleActorDeath(ActorDeathMessage actorDeath)
    {
        ArgumentNullException.ThrowIfNull(actorDeath.Actor.TimeEntity);
        RemoveEntity(actorDeath.Actor.TimeEntity);
    }

    public ulong GameTicks => _gameTicks;

    private ITimeEntity GetNextEntity()
    {
        var entity = _currentNode!.Value;

        do
        {
            //entity.Actor.TryGetComponent<StatsComponent>
        } while (false);

        return entity;
    }

    public void ProcessNext()
    {
        if (_entities.Count == 0)
        {
            _gainEnergy = false;
            return;
        }

        var entity = GetNextEntity();
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
                        entity.Actor.ConsumeAction();
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
        }

        _currentNode = _currentNode.Next ?? _entities.First;
    }

    public void AddEntity(ITimeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

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
