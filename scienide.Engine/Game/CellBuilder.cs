﻿using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public class CellBuilder
{
    private readonly Cell _cell;

    private CellBuilder(Point pos)
    {
        _cell = new Cell(pos);
    }

    public static CellBuilder CreateBuilder(Point pos) => new(pos);

    public CellBuilder AddTerrain(char glyph)
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
