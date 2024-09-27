using SadConsole;
using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.Game;

public class GameMap : IGameMap
{
    private FlatArray<GameComposite> _data;
    private ScreenSurface _surface;

    public GameMap(int width, int height)
    {
        Width = width;
        Height = height;

        _data = new FlatArray<GameComposite>(Width, Height);

        _surface = new ScreenSurface(Width, Height)
        {
            UseKeyboard = true,
            UseMouse = true
        };

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var cell = CellBuilder.GetBuilder(new(x, y))
                    .AddGlyph(',')
                    .WithParent(this)
                    .Build();
                _data[x, y] = cell;

                _surface.SetGlyph(x, y, cell.Glyph.Char);
            }
        }
    }

    public FlatArray<GameComposite> Data => _data;

    public Point Position { get; set; }

    public int Z { get; }

    public int Width { get; }

    public int Height { get; }

    public void Traverse(Action<IGameComponent> action)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                action(Data[x, y]);
            }
        }
    }

    public IGameComponent? Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Glyph Glyph { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ScreenSurface Surface => _surface;
}
