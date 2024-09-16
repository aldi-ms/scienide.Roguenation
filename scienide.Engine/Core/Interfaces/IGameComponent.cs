using scienide.Engine.Game;

namespace scienide.Engine.Core.Interfaces;

/// <summary>
/// The base component object in the hierarchy.
/// </summary>
public interface IGameComponent
{
    IGameComponent? Parent { get; set; }
    Glyph? Glyph { get; set; }

    void Traverse(Action<IGameComponent> action);
}
