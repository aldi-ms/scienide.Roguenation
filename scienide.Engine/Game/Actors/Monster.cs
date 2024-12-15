namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

public class Monster : Actor
{
    public Monster(Point pos, string name) : base(pos, name)
    {
        ObjectType = GObjType.NPC;
    }

    public Monster(Point pos) : this(pos, string.Empty)
    {
    }

    public override IActionCommand TakeTurn()
    {
        return new WalkAction(this, Global.GetRandomValidDirection());
    }

    public override IActor Clone(bool deepClone)
    {
        var monster = new Monster(Position, Name);

        if (deepClone)
        {
            monster.Glyph = Glyph.Clone(deepClone);
            if (TimeEntity != null)
                monster.TimeEntity = new ActorTimeEntity(TimeEntity.Energy, TimeEntity.Speed);

            // For hero.GameMap to be cloned we need to actually spawn the actor; don't do that for now
        }
        else
        {
            monster.Glyph = Glyph.Clone(deepClone);
        }

        return monster;
    }
}
