using SadRogue.Primitives;

namespace scienide.Engine.Game;

public class CellBuilder
{
    private Cell _cell;

    private CellBuilder()
    {
        _cell = new Cell();
    }

    public static CellBuilder CreateBuilder() => new();

    public CellBuilder WithLocation(Point p)
    {
        _cell.Location = p;
        return this;
    }

    public CellBuilder WithGlyph(Glyph glyph)
    {
        _cell.Glyph = glyph;
        return this;
    }

    public CellBuilder Apply(Action<Cell> action)
    {
        action(_cell);
        return this;
    }

    public Cell Build() => _cell;
}
