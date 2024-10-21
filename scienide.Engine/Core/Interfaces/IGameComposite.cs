namespace scienide.Engine.Core.Interfaces;

using SadRogue.Primitives;
using System.Collections.ObjectModel;

/// <summary>
/// A unit of map data, which holds all information needed for the map.
/// i.e. a Tile.
/// The composite object in the hierarchy
/// </summary>
public interface IGameComposite : IGameComponent
{
    /// <summary>
    /// The (X; Y) coordinates of the cell if it is part of a larger map
    /// </summary>
    Point Position { get; set; }

    /// <summary>
    /// <see cref="ReadOnlyCollection{T}"/>  of <see cref="IGameComponent"/> objects that are contained inside this <see cref="IGameComposite"/>.
    /// </summary>
    ReadOnlyCollection<IGameComponent> Children { get; }

    /// <summary>
    /// Try to get a child component of this type.
    /// </summary>
    /// <param name="gameObjType">Type of game object</param>
    /// <param name="component">The <see cref="IGameComponent"/>component or null if none is found.</param>
    /// <returns><c>True</c> if the component of this type is found, <c>false</c> otherwise.</returns>
    bool GetComponent<T>(GObjType gameObjType, out T? component) where T : class, IGameComponent;

    /// <summary>
    /// Add a child <see cref="IGameComponent"/> object to the <see cref="IGameComposite"/>
    /// </summary>
    /// <param name="child"></param>
    /// <returns><c>True</c> if we have a valid child and it was added. <c>False</c> otherwise.</returns>
    bool AddChild(IGameComponent child);

    /// <summary>
    /// Remove a child <see cref="IGameComponent"/> object from the <see cref="IGameComposite"/>
    /// </summary>
    /// <param name="child"></param>
    /// <returns><c>True</c> if we have a valid child and it was removed. <c>False</c> otherwise.</returns>
    bool RemoveChild(IGameComponent child);
}
