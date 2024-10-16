namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;

public class HeroBuilder
{
    private readonly Hero _actor;

    private HeroBuilder(Point pos)
    {
        _actor = new Hero(pos);
    }

    public static HeroBuilder CreateBuilder(Point pos) => new(pos);

    public HeroBuilder AddGlyph(char ch)
    {
        _actor.Glyph = new Glyph(ch);
        return this;
    }

    public HeroBuilder SetActorTimeEntity(int energy, int speed, int baseCost)
    {
        var entity = new ActorTimeEntity(energy, speed)
        {
            Cost = baseCost
        };

        _actor.TimeEntity = entity;
        return this;
    }

    public HeroBuilder SetHeroCrashTimeEntity()
    {
        _actor.TimeEntity = new CrashTurnTimeEntity();
        return this;
    }

    public Hero Build() => _actor;
}