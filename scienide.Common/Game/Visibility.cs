namespace scienide.Common.Game;

using SadRogue.Primitives;

public abstract class Visibility
{
    /// <summary>
    /// Compute the field of view from the point of origin given, in a circle-range with r=<see cref="rangeLimit"/>
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="rangeLimit"></param>
    /// <returns></returns>
    public abstract List<Cell> Compute(Point origin, int rangeLimit);
}
