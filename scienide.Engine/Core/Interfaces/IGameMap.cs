namespace scienide.Engine.Core.Interfaces;

using SadConsole;
using SadRogue.Primitives;
using scienide.Engine.Game;
using scienide.Engine.Infrastructure;

public interface IGameMap : IGameComponent
{
    int Width { get; }
    int Height { get; }
    int Z { get; }
    Cell this[int x, int y] { get; }
    Cell this[Point point] { get; }
    FlatArray<Cell> Data { get; }
    ScreenSurface Surface { get; }
    Point GetRandomSpawnPoint(GameObjType ofType);
    List<Cell> DirtyCells { get; }
}
