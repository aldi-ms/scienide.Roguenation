namespace scienide.Engine.Core.Interfaces;

using scienide.Engine.Game;
using scienide.Engine.Infrastructure;

/// <summary>
/// The base component object in the hierarchy.
/// </summary>
public interface IGameComponent
{
    GObjType ObjectType { get; }

    IGameComponent? Parent { get; set; }

    Glyph Glyph { get; set; }

    CollisionLayer Layer { get; }
}
