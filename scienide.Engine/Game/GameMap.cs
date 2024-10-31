namespace scienide.Engine.Game;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;

public class GameMap : IGameMap
{
    private readonly FlatArray<Cell> _data;
    private readonly ScreenSurface _surface;

    public GameMap(ScreenSurface surface)
    {
        _surface = surface;
        Width = _surface.Width;
        Height = _surface.Height;

        _data = new FlatArray<Cell>(Width, Height);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var cell = CellBuilder.CreateBuilder(new(x, y))
                    .AddTerrain(new Terrain(','))
                    .WithParent(this)
                    .Build();
                Data[x, y] = cell;

                Surface.SetGlyph(x, y, cell.Glyph.Char);
            }
        }
    }

    public Cell this[Point pos]
    {
        get => _data[pos];
        set => _data[pos] = value;
    }

    public Cell this[int x, int y]
    {
        get => _data[x, y];
        set => _data[x, y] = value;
    }

    public FlatArray<Cell> Data => _data;

    public Point Position { get; set; }

    public int Z { get; }

    public int Width { get; }

    public int Height { get; }

    public ScreenSurface Surface => _surface;

    public CollisionLayer Layer { get; set; } = CollisionLayer.Map;

    public List<Cell> DirtyCells { get; } = [];

    public GObjType ObjectType { get => GObjType.Map; set => throw new NotImplementedException(); }

    public Point GetRandomSpawnPoint(GObjType forObjectType)
    {
        int x, y;
        do
        {
            x = Global.RNG.Next(Width);
            y = Global.RNG.Next(Height);
        } while (!Data[x, y].IsValidForEntry(forObjectType));

        return new Point(x, y);
    }

    public IGameComponent? Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Glyph Glyph { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
