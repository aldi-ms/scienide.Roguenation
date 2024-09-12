namespace scienide.Engine.Core.Interfaces;

/// <summary>
/// The base component object in the hierarchy.
/// </summary>
public interface IGameComponent
{
    char Glyph { get; set; }
    IGameComponent? Parent { get; set; }

    void Traverse(Action<IGameComponent> action);
}
