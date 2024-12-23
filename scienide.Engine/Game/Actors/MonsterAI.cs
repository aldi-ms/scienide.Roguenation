namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

public class MonsterAI(IActor actor) : BaseAI(actor)
{
    public override IActionCommand Act()
    {
        if (Actor.GameMap == null)
        {
            return new WalkAction(Actor, Global.GetRandomValidDirection());
        }

        Cell[] dirtyCellsCopy = new Cell[Actor.GameMap.DirtyCells.Count];
        Actor.GameMap.DirtyCells.CopyTo(dirtyCellsCopy);

        var visibleCells = Actor.GameMap.FoV.Compute(Actor.Position, Actor.FoVRange);

        // Refactor this with an Actor state machine
        // Attacking/Angry, Lazy, Frightened, Normal
        var target = GetTarget(visibleCells);

        if (target != null)
        {
            var dir = Direction.GetDirection(Actor.Position, target.Position);
            return new WalkAction(Actor, dir);
        }

        return new WalkAction(Actor, Global.GetRandomValidDirection());
    }

    private static Cell? GetTarget(List<Cell> cells)
    {
        return cells.Where(x => x.Actor?.TypeId == Global.HeroId).FirstOrDefault();
    }
}
