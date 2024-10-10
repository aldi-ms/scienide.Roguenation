using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public struct Terrain(char glyph) : IGameComponent
{
    public readonly CollisionLayer Layer => CollisionLayer.Terrain;

    public Glyph Glyph { get; set; } = new Glyph(glyph);

    public IGameComponent? Parent { get; set; }

    public void Traverse(Action<IGameComponent> action)
    {
        throw new NotImplementedException();
    }
}
