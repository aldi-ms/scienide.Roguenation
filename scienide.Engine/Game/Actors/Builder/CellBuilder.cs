namespace scienide.Engine.Game.Actors.Builder;

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
        if (GlyphBeautifier.GlyphAppearanceMap.TryGetValue(ch, out var appearance))
        {
            glyph = new Glyph(appearance);
        }

        return SetTerrainGlyph(glyph);
    }

    public CellBuilder SetTerrainGlyph(Glyph glyph)
    {
        _cell.Terrain = new Terrain(glyph);
        return this;
    }

    public CellBuilder WithParent(IGameMap map)
    {
        _cell.Parent = map;
        return this;
    }

    public Cell Build() => _cell;
}
