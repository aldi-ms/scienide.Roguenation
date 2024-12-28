namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;
using System.Diagnostics.CodeAnalysis;

public struct Terrain(Glyph glyph) : IGameComponent
{
    public readonly CollisionLayer Layer => CollisionLayer.Terrain;

    public readonly GObjType ObjectType => GObjType.Terrain;

    public Glyph Glyph { get; set; } = glyph;

    public IGameComponent? Parent { get; set; }

    public override readonly bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Terrain casted)
        {
            return Glyph.Equals(casted.Glyph);
        }

        return false;
    }

    public override readonly int GetHashCode()
    {
        return Glyph.GetHashCode();
    }

    public static bool operator ==(Terrain left, Terrain right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Terrain left, Terrain right)
    {
        return !(left == right);
    }
}
