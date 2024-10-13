using SadRogue.Primitives;

namespace scienide.Engine.Game.Actors;

public class HeroBuilder
{
    private readonly Hero _hero;
    private readonly Point _pos;

    private HeroBuilder(Point pos)
    {
        _pos = pos;
        _hero = new Hero(_pos);
    }

    public static HeroBuilder CreateBuilder(Point pos) => new(pos);

    public HeroBuilder AddGlyph(char ch)
    {
        _hero.Glyph = new Glyph(ch);
        return this;
    }

    public HeroBuilder SetHeroTimeEntity(int energy, int speed, int baseCost)
    {
        var entity = new ActorTimeEntity(energy, speed)
        {
            Cost = baseCost
        };

        _hero.TimeEntity = entity;
        return this;
    }

    public HeroBuilder SetHeroCrashTimeEntity()
    {
        _hero.TimeEntity = new CrashTurnTimeEntity();
        return this;
    }

    public Hero Build() => _hero;
}