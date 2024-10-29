namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;

public struct Terrain(char glyph) : IGameComponent
{
    public Glyph Glyph { get; set; } = new Glyph(glyph);

    public readonly CollisionLayer Layer => CollisionLayer.Terrain;

    public IGameComponent? Parent { get; set; }

    public readonly GObjType ObjectType => GObjType.Terrain;

    public void Traverse(Action<IGameComponent> action)
    {
        throw new NotImplementedException();
    }
}
