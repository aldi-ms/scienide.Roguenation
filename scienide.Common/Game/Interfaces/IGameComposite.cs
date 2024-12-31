namespace scienide.Common.Game.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A unit of map data, which holds all information needed for the map.
/// i.e. a Tile.
/// The composite object in the hierarchy
/// </summary>
public interface IGameComposite : IGameComponent
{
    /// <summary>
    /// Add a child <see cref="IGameComponent"/> object to the <see cref="IGameComposite"/>.
    /// </summary>
    /// <typeparam name="T">Type of component to add, needs to implement <see cref="IGameComponent"/></typeparam>
    /// <param name="component"></param>
    /// <returns><c>True</c> if we have a valid child and it was added, <c>false</c> otherwise.</returns>
    bool AddComponent<T>(T component) where T : IGameComponent;

    /// <summary>
    /// Try to get all child components of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of component to look for, needs to implement <see cref="IGameComponent"/></typeparam>
    /// <param name="components">The <see cref="IGameComponent"/>components or null if none are found.</param>
    /// <returns><c>True</c> if component/s of this type are found, <c>false</c> otherwise.</returns>
    bool TryGetComponents<T>([NotNullWhen(true)] out IEnumerable<T>? components) where T : IGameComponent;

    /// <summary>
    /// Try to get a child component.
    /// </summary>
    /// <typeparam name="T">Type of component to look for, needs to implement <see cref="IGameComponent"/></typeparam>
    /// <param name="component">The <see cref="IGameComponent"/>component or null if none is found.</param>
    /// <param name="searchRecursive">Set to <c>true</c> to search recursively in any composite components that are contained. Default is <c>false</c>.</param>
    /// <returns><c>True</c> if the component of this type is found, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throw if more than 1 components of type <typeparamref name="T"/> are found.</exception>
    bool TryGetComponent<T>([NotNullWhen(true)] out T? component, bool searchRecursive = false) where T : IGameComponent;

    /// <summary>
    /// Remove a child <typeparamref name="T"/> object from the <see cref="IGameComposite"/>.
    /// </summary>
    /// <typeparam name="T">Type of component remove, needs to implement <see cref="IGameComponent"/></typeparam>
    /// <param name="component">The component that has to be removed.</param>
    /// <returns><c>True</c> if we have a valid child and it was removed, <c>false</c> otherwise.</returns>
    bool RemoveComponent<T>(T component) where T : IGameComponent;

    /// <summary>
    /// Remove all child objects with type <typeparamref name="T"/> from the <see cref="IGameComposite"/>.
    /// </summary>
    /// <typeparam name="T">Type of components remove, needs to implement <see cref="IGameComponent"/></typeparam>
    /// <returns><c>True</c> if we have at least one valid child and it was removed, <c>false</c> otherwise.</returns>
    bool RemoveComponents<T>() where T : IGameComponent;

    /// <summary>
    /// <see cref="ReadOnlyCollection{IGameComponent}"/> objects that are contained inside this <see cref="IGameComposite"/>.
    /// </summary>
    ReadOnlyCollection<IGameComponent> Components { get; }
}
