using SadConsole;
using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actors;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.Game;

public class GameMap : IGameMap
{
    private readonly FlatArray<Cell> _data;
    private readonly ScreenSurface _surface;

    public GameMap(int width, int height)
    {
        Width = width;
        Height = height;

        _data = new FlatArray<Cell>(Width, Height);
        _surface = new ScreenSurface(Width, Height)
        {
            UseKeyboard = true,
            UseMouse = true
        };

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var cell = CellBuilder.CreateBuilder(new(x, y))
                    .AddTerrain(',')
                    .WithParent(this)
                    .Build();
                Data[x, y] = cell;

                Surface.SetGlyph(x, y, cell.Glyph.Char);
            }
        }

        var heroSpawn = GetRandomSpawnPoint(GameObjType.ActorPlayerControl);
        var hero = HeroBuilder.CreateBuilder(heroSpawn)
            .AddGlyph('@')
            .AddTimedEntity(100, 100, 50)
            .Build();
        Data[heroSpawn].AddChild(hero);
        Surface.SetGlyph(heroSpawn.X, heroSpawn.Y, Data[heroSpawn].Glyph.Char);
    }

    public FlatArray<Cell> Data => _data;

    public Point Position { get; set; }

    public int Z { get; }

    public int Width { get; }

    public int Height { get; }

    public ScreenSurface Surface => _surface;

    public CollisionLayer Layer { get; set; } = CollisionLayer.Map;


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

    public Point GetRandomSpawnPoint(GameObjType ofType)
    {
        int x, y;
        do
        {
            x = Global.RNG.Next(Width);
            y = Global.RNG.Next(Height);
        } while (!Data[x, y].IsValidForEntry(ofType));

        return new Point(x, y);
    }

    public IGameComponent? Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Glyph Glyph { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
