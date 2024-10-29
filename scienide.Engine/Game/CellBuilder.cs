namespace scienide.Engine.Game;

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

    public CellBuilder AddTerrain(Terrain t)
    {
        _cell.Terrain = t;
        return this;
    }

    public CellBuilder WithParent(IGameMap map)
    {
        _cell.Parent = map;
        return this;
    }

    public Cell Build() => _cell;
}
