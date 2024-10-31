namespace scienide.Engine.Game.Actors.Builder;

using SadRogue.Primitives;
using scienide.Common.Game;

public abstract class ActorBuilder
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    protected Actor _actor;
#pragma warning restore CS8618

    public virtual Actor Build() => _actor;

    public virtual ActorBuilder SetTimeEntity(TimeEntity timeEntity)
    {
        _actor.TimeEntity = timeEntity;
        return this;
    }

    public virtual ActorBuilder SetName(string name)
    {
        _actor.Name = name;
        return this;
    }

    public virtual ActorBuilder SetGlyph(char ch)
    {
        _actor.Glyph = new Glyph(ch);
        return this;
    }
}

public class HeroBuilder : ActorBuilder
{
    public HeroBuilder(Point pos)
    {
        _actor = new Hero(pos);
        _actor.ObjectType = GObjType.ActorPlayerControl;
    }
}

public class MonsterBuilder : ActorBuilder
{
    public MonsterBuilder(Point pos)
    {
        _actor = new Monster(pos);
        _actor.ObjectType = GObjType.ActorNonPlayerControl;
    }
}