namespace scienide.Engine.Game.Actors.Builder;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

public class CellBuilder
{
    private readonly Cell _cell;

    private CellBuilder(Point pos)
    {
        _cell = new Cell(pos);
    }

    public static CellBuilder CreateBuilder(Point pos) => new(pos);

    public CellBuilder SetTerrainGlyph(char ch)
    {
        var glyph = new Glyph(ch);
        if (GlyphData.GlyphAppearanceMap.TryGetValue(ch, out var appearance))
        {
            glyph = new Glyph((ColoredGlyphAndEffect)appearance.Clone());
        }

        return SetTerrainGlyph(glyph);
    }

    public CellBuilder SetTerrainGlyph(ColoredGlyphAndEffect glyph)
    {
        return SetTerrainGlyph(new Glyph(glyph));
    }

    public CellBuilder WithParent(IGameMap map)
    {
        ArgumentNullException.ThrowIfNull(map);

        _cell.Parent = map;
        return this;
    }

    public Cell Build() => _cell;

    private CellBuilder SetTerrainGlyph(Glyph glyph)
    {
        _cell.Terrain = new Terrain(glyph);
        _cell.Properties[Common.Infrastructure.Props.IsWalkable] = glyph.Char != '#';

        return this;
    }
}
