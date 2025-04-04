﻿namespace scienide.Engine.Game;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;
using scienide.Common.Logging;
using scienide.Engine.FieldOfView;
using scienide.Engine.Game.Actors.Builder;
using Serilog;

public class GameMap : IGameMap
{
    private readonly FlatArray<Cell> _data;
    private readonly ScreenSurface _surface;
    private readonly Visibility _fov;

    public GameMap(ScreenSurface surface, FlatArray<Glyph> mapData)
    {
        var logConfig = new LoggerConfiguration()
            .Destructure.ByTransforming<IActor>(x => new { Id = x.Id, x.Name })
            .WriteTo.File("Logs\\Game.log")
            .WriteTo.Debug()
            .MinimumLevel.Information();
        GameLogger = Logging.ConfigureNamedLogger($"Logs\\Game-{DateTime.Today:yy-MM-dd}.log", logConfig);

        GameLogger.Information($"=== Starting GameMap ===");

        _surface = surface;
        Width = _surface.Width;
        Height = _surface.Height;

        if (mapData == null || mapData.Width != Width || mapData.Height != Height)
        {
            throw new ArgumentOutOfRangeException(nameof(mapData));
        }

        _data = new FlatArray<Cell>(Width, Height);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var cell = CellBuilder.CreateBuilder(new(x, y))
                    .SetTerrainGlyph(mapData[x, y].Appearance.GlyphCharacter)
                    .WithParent(this)
                    .Build();
                _data[x, y] = cell;

#if !ENABLE_FOV

                Surface.SetCellAppearance(x, y, mapData[x, y].Appearance);
#endif
            }
        }

#if ENABLE_FOV
        _fov = new MyVisibility(this);
#else
        _fov = VisibilityEmpty.Instance;
#endif
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

    public Layer Layer { get; set; } = Layer.Map;

    public HashSet<Cell> DirtyCells { get; } = [];

    public GObjType ObjectType { get => GObjType.Map; set => throw new NotImplementedException(); }

    public Visibility FoV => _fov;

    public ILogger GameLogger { get; private set; }

    public Point GetRandomSpawnPoint(GObjType gObjType)
    {
        int x, y;
        do
        {
            x = Global.RNG.Next(Width);
            y = Global.RNG.Next(Height);
        } while (!Data[x, y].IsValidCellForEntry(gObjType));

        return new Point(x, y);
    }

    public bool IsInValidMapBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public static void HighlightPath(GameMap map, Point[] path)
    {
        for (var i = 0; i < path.Length; i++)
        {
            map[path[i]].Properties[Props.Highlight] = true;
            map.DirtyCells.Add(map[path[i]]);
        }
    }

    public IGameComponent? Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Glyph Glyph { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Status => null!;
}
