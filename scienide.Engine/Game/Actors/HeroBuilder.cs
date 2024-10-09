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

    public HeroBuilder AddTimedEntity(int energy, int speed, int baseCost)
    {
        var entity = new HeroTimedEntity(_hero)
        {
            Energy = energy,
            Speed = speed,
            Cost = baseCost
        };

        _hero.TimedEntity = entity;
        return this;
    }

    public Hero Build() => _hero;
}