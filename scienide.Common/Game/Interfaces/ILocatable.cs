namespace scienide.Common.Game.Interfaces;

using SadRogue.Primitives;

public interface ILocatable
{
    /// <summary>
    /// The (X; Y) coordinates of the cell in part of a larger map/grid.
    /// </summary>
    Point Position { get; set; }
}