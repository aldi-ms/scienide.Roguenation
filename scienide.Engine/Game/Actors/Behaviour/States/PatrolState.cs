namespace scienide.Engine.Game.Actors.Behaviour.States;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

internal class PatrolState : StateBase
{
    private readonly Point _homePoint;
    private Point _targetPoint;
    private bool _going = false;

    public PatrolState(IActor actor) : base(actor)
    {
        State = MonsterState.Patrol;
        _homePoint = actor.Position;
        _targetPoint = actor.Position;
    }

    public override IActionCommand Act(Cell[] cells)
    {
        if (_targetPoint == Actor.Position)
        {
            _going = !_going;

            if (_going)
            {
                var target = Actor.CurrentCell;
                var currentDistance = 0f;
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i].IsValidForEntry(GObjType.NPC))
                    {
                        var d = Utils.EuclideanDistance(_homePoint, cells[i].Position);
                        if (d > currentDistance)
                        {
                            currentDistance = d;
                            target = cells[i];
                        }
                    }
                }

                _targetPoint = target.Position;
            }
            else
            {
                _targetPoint = _homePoint;
            }
        }

        var dir = Direction.GetDirection(Actor.Position, _targetPoint);
        return new WalkAction(Actor, dir);
    }
}
