using SadConsole;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.Core.Interfaces;

public interface IGameMap : IGameComponent
{
    int Width { get; }
    int Height { get; }
    int Z { get; }
    FlatArray<GameComposite> Data { get; }
    ScreenSurface Surface { get; }
}
