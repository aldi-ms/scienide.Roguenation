using SadRogue.Primitives;

namespace scienide.Engine;

public static class Global
{
    public static Random RNG = new();

    public static Direction GetRandomValidDirection()
    {
        int dX, dY;
        do
        {
            dX = Global.RNG.Next(-1, 2);
            dY = Global.RNG.Next(-1, 2);
        }
        while (false);

        return Direction.GetCardinalDirection(dX, dY);
    }
}
