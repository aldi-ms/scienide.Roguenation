using SadRogue.Primitives;
using System.Collections.ObjectModel;

namespace scienide.Engine.Core.Interfaces;

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
    public Point Location { get; set; }

    /// <summary>
    /// <see cref="ReadOnlyCollection{T}"/>  of <see cref="IGameComponent"/> objects that are contained inside this <see cref="IGameComposite"/>.
    /// </summary>
    public ReadOnlyCollection<IGameComponent> Children { get; }

    /// <summary>
    /// Add a child <see cref="IGameComponent"/> object to the <see cref="IGameComposite"/>
    /// </summary>
    /// <param name="child"></param>
    /// <returns><c>True</c> if we have a valid child and it was added. <c>False</c> otherwise.</returns>
    public bool AddChild(IGameComponent child);

    /// <summary>
    /// Remove a child <see cref="IGameComponent"/> object from the <see cref="IGameComposite"/>
    /// </summary>
    /// <param name="child"></param>
    /// <returns><c>True</c> if we have a valid child and it was removed. <c>False</c> otherwise.</returns>
    public bool RemoveChild(IGameComponent child);
}
