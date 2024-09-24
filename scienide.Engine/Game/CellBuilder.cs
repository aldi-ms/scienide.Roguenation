using SadRogue.Primitives;

namespace scienide.Engine.Game;

public class CellBuilder
{
    private Cell _cell;

    private CellBuilder(Point pos)
    {
        _cell = new Cell(pos);
    }

    public static CellBuilder CreateBuilder(Point pos) => new(pos);

    //public CellBuilder WithPosition()
    //{
    //    _cell.Position = p;
    //    return this;
    //}

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
