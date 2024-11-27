namespace scienide.Common;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

public static class MapUtils
{
    private static readonly List<Color> _regionColors = typeof(Color).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
        .Where(x => x.FieldType == typeof(Color))
#pragma warning disable CS8605 // Unboxing a possibly null value.
        .Select(static x => (Color)x.GetValue(null))
#pragma warning restore CS8605 // Unboxing a possibly null value.
        .ToList();

    public static void ColorizeRegions(IGameMap map, List<List<Cell>> regions)
    {
        foreach (var cellRegion in regions)
        {
            var color = _regionColors[Global.RNG.Next(_regionColors.Count)];
            foreach (var cell in cellRegion)
            {
                cell.Glyph.Appearance.Background = color;
                cell.Glyph.Appearance.IsDirty = true;
                map.DirtyCells.Add(cell);
            }
        }
    }
}