namespace scienide.Common.Map;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

public static class MapUtils
{
#pragma warning disable CS8605 // Getting a static property
    private static readonly List<Color> _regionColors = typeof(Color).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
        .Where(x => x.FieldType == typeof(Color))
        .Select(static x => (Color)x.GetValue(null))
        .ToList();
#pragma warning restore CS8605

    public static void ColorizeRegions(IGameMap map, List<RegionCellData> regions)
    {
        foreach (var regionData in regions)
        {
            var color = _regionColors[Global.RNG.Next(_regionColors.Count)];
            foreach (var cell in regionData.Cells)
            {
                cell.Glyph.Appearance.Background = color;
                map.DirtyCells.Add(cell);
            }

            foreach (var borderCell in regionData.Borders)
            {
                borderCell.Glyph.Appearance.Foreground = color;
                map.DirtyCells.Add(borderCell);
            }
        }
    }

    public static List<Cell> GetCellsWithinDistance(IGameMap map, Point center, int distance)
    {
        var sqDistance = distance * distance;
        var cells = new List<Cell>();

        for (int x = center.X - distance; x <= center.X + distance; x++)
        {
            for (int y = center.Y - distance; y <= center.Y + distance; y++)
            {
                if (!map.IsInValidMapBounds(x, y) || (x == center.X && y == center.Y))
                {
                    continue;
                }

                var dx = x - center.X;
                var dy = y - center.Y;

                if (dx * dx + dy * dy <= sqDistance)
                {
                    cells.Add(map[x, y]);
                }
            }
        }

        return cells;
    }
}