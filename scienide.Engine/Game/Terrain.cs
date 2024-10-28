namespace scienide.Engine.Game;

using scienide.Engine.Core.Interfaces;
using scienide.Engine.Infrastructure;

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
