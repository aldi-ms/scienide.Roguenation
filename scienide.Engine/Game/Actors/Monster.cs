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
        //if (Actor.GameMap == null)
        {
            return new WalkAction(Actor, Global.GetRandomValidDirection());
        }
        
    }
}

public class Monster : Actor
{
    private BaseAI _ai;

    public Monster(Point pos, string name) : base(pos, name)
    {
        ObjectType = GObjType.NPC;
        _ai = new MonsterAI(this);
    }

    public Monster(Point pos) : this(pos, string.Empty)
    {
    }

    public BaseAI AI => _ai;

    public override IActionCommand TakeTurn()
    {
        return _ai.Act();
    }

    public override IActor Clone(bool deepClone)
    {
        // Don't use MonsterBuilder here, as it can't actually use the cloned Glyph
        var monster = new Monster(Position, Name);

        if (deepClone)
        {
            monster.Glyph = Glyph.Clone(deepClone);
            if (TimeEntity != null)
                monster.TimeEntity = new ActorTimeEntity(TimeEntity.Energy, TimeEntity.Speed);

            // For hero.GameMap to be cloned we need to actually spawn the actor; don't do that for now
            // i.e. don't clone actual game-entity
        }
        else
        {
            monster.Glyph = Glyph.Clone(deepClone);
        }

        return monster;
    }
}
