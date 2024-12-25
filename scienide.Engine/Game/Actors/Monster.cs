namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actors.Behaviour;

public class Monster : Actor
{
    private readonly BaseAI _ai;

    public  Monster(Point pos, string name) : base(pos, name)
    {
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
        var monster = new Monster(Position, Name)
        {
            ObjectType = this.ObjectType
        };

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
