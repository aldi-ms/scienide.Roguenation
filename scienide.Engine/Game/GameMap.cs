﻿namespace scienide.Engine.Game;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;
using scienide.Engine.Game.Actors.Builder;

public class GameMap : IGameMap
{
    private readonly FlatArray<Cell> _data;
    private readonly ScreenSurface _surface;

    public GameMap(ScreenSurface surface, FlatArray<Glyph> mapData, bool initialDrawMap)
    {
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

                if (initialDrawMap)
                {
                    Surface.SetCellAppearance(x, y, mapData[x, y].Appearance);
                }
            }
        }
    }

    public Cell this[Point pos]
    {
        get
        {
            if (IsInBounds(pos.X, pos.Y))
            {
                return _data[pos];
            }
#pragma warning disable CS8603
            return default;
#pragma warning restore CS8603
        }
        set
        {
            if (IsInBounds(pos.X, pos.Y))
            {
                _data[pos] = value;
            }
        }
    }

    public Cell this[int x, int y]
    {
        get
        {
            if (IsInBounds(x, y))
            {
                return _data[x, y];
            }
#pragma warning disable CS8603
            return default;
#pragma warning restore CS8603
        }
        set
        {
            if (IsInBounds(x, y))
            {
                _data[x, y] = value;
            }
        }
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

    private bool IsInBounds(int x, int y)
    {
        return (x >= 0 && x < Width && y >= 0 && y < Height);
    }

    public IGameComponent? Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Glyph Glyph { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
