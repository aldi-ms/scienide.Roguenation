namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Components;
using scienide.Engine.Game.Actors.Behaviour;

public class Monster : Actor
{
    private readonly BehaviourBase _behaviour;

    public Monster(Point pos, string name) : base(pos, name)
    {
        // For now keep a ref of the behaviour in two places
        // in a class field & in components
        // might not be the best, but _behaviour is used on each turn
        _behaviour = new MonsterBehaviour(this);
        AddComponent(_behaviour);
    }

    public Monster(Point pos) : this(pos, string.Empty)
    {
    }

    internal BehaviourBase Behaviour => _behaviour;

    public override IActionCommand TakeTurn()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        return _behaviour.Act();
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
                monster.TimeEntity = new TimeEntity(TimeEntity.Energy, TimeEntity.Speed);

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
