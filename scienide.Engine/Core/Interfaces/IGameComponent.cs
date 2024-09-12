using scienide.Engine.Game;

namespace scienide.Engine.Core.Interfaces;

/// <summary>
/// The base component object in the hierarchy.
/// </summary>
public interface IGameComponent
{
    Glyph Glyph { get; set; }
    IGameComponent? Parent { get; set; }

    void Traverse(Action<IGameComponent> action);
}
