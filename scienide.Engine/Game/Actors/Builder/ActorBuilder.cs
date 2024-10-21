namespace scienide.Engine.Game.Actors.Builder;

using SadRogue.Primitives;

public abstract class ActorBuilder
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    protected Actor _actor;
#pragma warning restore CS8618

    public abstract Actor Build();
    public abstract ActorBuilder SetName(string name);
    public abstract ActorBuilder SetTimeEntity(TimeEntity timeEntity);
    public abstract ActorBuilder SetGlyph(char ch);
}

public class HeroBuilder : ActorBuilder
{
    public HeroBuilder(Point pos)
    {
        _actor = new Hero(pos);
    }

    public override Actor Build() => _actor;

    public override ActorBuilder SetTimeEntity(TimeEntity timeEntity)
    {
        _actor.TimeEntity = timeEntity;
        return this;
    }

    public override ActorBuilder SetName(string name)
    {
        _actor.Name = name;
        return this;
    }

    public override ActorBuilder SetGlyph(char ch)
    {
        _actor.Glyph = new Glyph(ch);
        return this;
    }
}

public class MonsterBuilder : ActorBuilder
{
    public MonsterBuilder(Point pos)
    {
        _actor = new Monster(pos);
    }

    public override Actor Build() => _actor;

    public override ActorBuilder SetTimeEntity(TimeEntity timeEntity)
    {
        _actor.TimeEntity = timeEntity;
        return this;
    }

    public override ActorBuilder SetName(string name)
    {
        _actor.Name = name;
        return this;
    }

    public override ActorBuilder SetGlyph(char ch)
    {
        _actor.Glyph = new Glyph(ch);
        return this;
    }
}