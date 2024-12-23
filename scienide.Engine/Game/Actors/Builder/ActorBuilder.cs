﻿namespace scienide.Engine.Game.Actors.Builder;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common.Game;
using System.Diagnostics.CodeAnalysis;

public abstract class ActorBuilder
{
    [AllowNull]
    protected Actor _actor;

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
        if (GlyphData.GlyphAppearanceMap.TryGetValue(ch, out var glyphAppearance))
        {
            _actor.Glyph = new Glyph((ColoredGlyphAndEffect)glyphAppearance.Clone());
        }
        else
        {
            _actor.Glyph = new Glyph(ch);
        }

        return this;
    }

    public virtual ActorBuilder SetFoVRange(int viewRange)
    {
        _actor.FoVRange = viewRange;
        return this;
    }
}

public sealed class HeroBuilder : ActorBuilder
{
    public HeroBuilder(Point pos)
    {
        _actor = new Hero(pos);
    }
}

public sealed class MonsterBuilder : ActorBuilder
{
    public MonsterBuilder(Point pos)
    {
        _actor = new Monster(pos)
        {
            ObjectType = GObjType.NPC
        };
    }
}