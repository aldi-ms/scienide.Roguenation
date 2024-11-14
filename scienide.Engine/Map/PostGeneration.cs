namespace scienide.Engine.Map;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Infrastructure;
using scienide.Engine.Game;

public class PostGeneration
{
    public GameMap FloodFillAndConnect(GameMap map)
    {
        do
        {
            var pos = GetRandomUnfilledPosition(map);
            map[pos].Properties[Props.IsFloodFilled] = true;
            var neighbours = map[pos].GetValidNeighbors();

            foreach (var neighbour in neighbours)
            {
                neighbour.Properties[Props.IsFloodFilled] = true;
            }

            var a = map[Point.Zero].Properties[Props.IsOpaque];
            break;
        } while (true);

        return map;
    }

    // needs to be reworked to return from the current unfilled position
    private Point GetRandomUnfilledPosition(GameMap map)
    {
        var runs = 0;
        var point = Point.None;
        do
        {
            runs++;
            point = new Point(Global.RNG.Next(0, map.Width), Global.RNG.Next(0, map.Height));
        } while ((map.IsInValidMapBounds(point) && map[point].Glyph != '#' && !map[point].Properties[Props.IsFloodFilled]) || runs > 30);

        if (point == Point.None)
        {
            throw new ArgumentException($"{nameof(GetRandomUnfilledPosition)} could not fetch a valid random point in 30+ runs.", nameof(point));
        }

        return point;
    }
}
