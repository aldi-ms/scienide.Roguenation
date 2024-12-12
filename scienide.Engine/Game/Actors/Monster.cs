namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;
using System.Diagnostics;

public class Monster : Actor
{
    public Monster(Point pos) : base(pos)
    {
        ObjectType = GObjType.NPC;
    }

    public override IActionCommand TakeTurn()
    {
        return new WalkAction(this, Global.GetRandomValidDirection());
    }

    public override IActor Clone(bool deepClone)
    {
        var monster = new Monster(Position);
        monster.Name = Name;

        if (deepClone)
        {
            monster.Glyph = Glyph.Clone(deepClone);
            // For hero.GameMap to be cloned we need to actually spawn the actor? don't do that for now
            if (TimeEntity != null)
                monster.TimeEntity = new ActorTimeEntity(TimeEntity.Energy, TimeEntity.Speed);
        }
        else
        {
            monster.Glyph = Glyph.Clone(deepClone);
        }

        return monster;
    }
}
