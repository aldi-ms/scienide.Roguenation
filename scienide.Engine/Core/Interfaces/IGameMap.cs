using SadConsole;
using SadRogue.Primitives;
using scienide.Engine.Game;
using scienide.Engine.Game.Actors;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.Core.Interfaces;

public interface IGameMap : IGameComponent
{
    int Width { get; }
    int Height { get; }
    int Z { get; }
    FlatArray<Cell> Data { get; }
    ScreenSurface Surface { get; }
    Point GetRandomSpawnPoint(GameObjType ofType);
}
