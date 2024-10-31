﻿namespace scienide.Common.Game;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;

public interface IGameMap : IGameComponent
{
    int Width { get; }
    int Height { get; }
    int Z { get; }
    Cell this[int x, int y] { get; }
    Cell this[Point point] { get; }
    FlatArray<Cell> Data { get; }
    ScreenSurface Surface { get; }
    Point GetRandomSpawnPoint(GObjType ofType);
    List<Cell> DirtyCells { get; }
}