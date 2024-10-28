namespace scienide.Common.Messaging;

using SadRogue.Primitives;

public interface ILocatable
{
    /// <summary>
    /// The (X; Y) coordinates of the cell in part of a larger map/grid.
    /// </summary>
    Point Position { get; set; }
}

public interface IMessageSubscriber : ILocatable
{

}