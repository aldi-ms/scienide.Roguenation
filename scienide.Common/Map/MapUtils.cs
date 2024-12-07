namespace scienide.Common.Map;

using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;

public static class MapUtils
{
    private static readonly List<Color> _regionColors = typeof(Color).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
        .Where(x => x.FieldType == typeof(Color))
#pragma warning disable CS8605 // Getting a static property
        .Select(static x => (Color)x.GetValue(null))
#pragma warning restore CS8605
        .ToList();

    public static void ColorizeRegions(IGameMap map, List<RegionCellData> regions)
    {
        foreach (var regionData in regions)
        {
            var color = _regionColors[Global.RNG.Next(_regionColors.Count)];
            foreach (var cell in regionData.Cells)
            {
                cell.Glyph.Appearance.Background = color;
                //cell.Glyph.Appearance.IsDirty = true;
                map.DirtyCells.Add(cell);
            }

            foreach (var borderCell in regionData.Borders)
            {
                borderCell.Glyph.Appearance.Foreground = color;
                //borderCell.Glyph.Appearance.IsDirty = true;
                map.DirtyCells.Add(borderCell);
            }
        }
    }
}