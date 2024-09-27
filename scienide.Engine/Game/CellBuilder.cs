using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public class CellBuilder
{
    private Cell _cell;

    private CellBuilder(Point pos)
    {
        _cell = new Cell(pos);
    }
    
    public static CellBuilder GetBuilder(Point pos) => new(pos);

    public CellBuilder AddGlyph(char ch)
    {
        _cell.Glyph = new Glyph(ch, _cell.Position)
        {
            // This creates a circular ref: Cell has a Glyph -> Glyph has a Parent:Cell
            //Parent = _cell
        };
        return this;
    }

    public CellBuilder WithParent(IGameMap map)
    {
        _cell.Parent = map;
        return this;
    }

    public Cell Build() => _cell;
}
