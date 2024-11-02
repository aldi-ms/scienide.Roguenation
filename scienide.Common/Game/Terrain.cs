namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;

public struct Terrain(Glyph glyph) : IGameComponent
{
    public readonly CollisionLayer Layer => CollisionLayer.Terrain;

    public readonly GObjType ObjectType => GObjType.Terrain;

    public Glyph Glyph { get; set; } = glyph;

    public IGameComponent? Parent { get; set; }

    public void Traverse(Action<IGameComponent> action)
    {
        throw new NotImplementedException();
    }
}
