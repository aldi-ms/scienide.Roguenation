namespace scienide.WaveFunctionCollapse;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Infrastructure;
using System.Collections;
using System.Diagnostics;

public class RegionMap : IEnumerable<RegionData>
{
    private bool _isInitialized = false;
    private readonly Dictionary<Ulid, RegionData> _inputRegionMap;
    private readonly Dictionary<Point, RegionData> _regionsMap;
    private readonly RegionData[] _regionArray;
    private readonly int _regionsWidth;
    private readonly int _regionsHeight;
    private readonly int _regionSize;

    public int Count => _regionArray.Length;

    public RegionData this[Point p]
    {
        get => _regionsMap[p];
        set => _regionsMap[p] = value;
    }

    public RegionMap(FlatArray<char> map, int regionSize)
    {
        _regionsMap = [];
        _regionSize = regionSize;
        _inputRegionMap = new Dictionary<Ulid, RegionData>();
        var regions = SplitToRegions(map, regionSize);
        _regionArray = [.. regions];
        for (int i = 0; i < _regionArray.Length; i++)
        {
            _regionsMap.Add(regions[i].GridCoordinates, regions[i]);
        }

        _regionsWidth = map.Width / _regionSize;
        _regionsHeight = map.Height / _regionSize;
    }

    public void Initialize(RegionMap input)
    {
        _inputRegionMap.Clear();
        var fullOptionsList = input.Select(x => x.Id).ToList();

        foreach (var option in input)
        {
            _inputRegionMap.Add(option.Id, option);
        }

        foreach (var region in this)
        {
            region.Options = fullOptionsList;
        }

        _isInitialized = true;
    }

    public ValidationResult IsValidMap()
    {
        if (!_isInitialized)
            return new ValidationResult(false, "Map is not initialized!");

        foreach (var region in this)
        {
            if (!region.IsCollapsed)
                return new ValidationResult(false, $"A region in the map is not collapsed: [{region.GridCoordinates}]:[{region.Id}].");
        }

        return ValidateCells();
    }

    public Dictionary<Direction, Point> GetValidNeighborPosition(Point ofRegion)
    {
        var result = new Dictionary<Direction, Point>(4);
        foreach (var dir in Global.DeltaCardinalNeighborDir)
        {
            var neighborPosition = ofRegion + dir;
            if (neighborPosition.X < 0 || neighborPosition.X >= _regionsWidth
                || neighborPosition.Y < 0 || neighborPosition.Y >= _regionsHeight
                || this[neighborPosition].IsCollapsed)
            {
                continue;
            }

            result.Add(Direction.GetCardinalDirection(dir), neighborPosition);
        }

        return result;
    }

    public RegionData CollapseRegionTo(Point regionPosition, Ulid optionId)
    {
        if (!_isInitialized)
        {
            throw new TypeInitializationException(nameof(RegionData), null);
        }

        //Trace.WriteLine($"[{nameof(CollapseRegionTo)}] region at:[{regionPosition}] to option:[{optionId}].");

        this[regionPosition].Options = [optionId];
        this[regionPosition].Map = _inputRegionMap[optionId].Map;

        var validNeighbors = GetValidNeighborPosition(regionPosition);
        foreach (var dir in validNeighbors.Keys)
        {
            var neighborPosition = validNeighbors[dir];
            var neighborRegion = this[neighborPosition];
            var directionToRegion = Direction.GetDirection(regionPosition - neighborPosition);
            var regionSide = this[regionPosition].Sides[dir];
            neighborRegion.Options = GetValidRegions(neighborRegion.Options, regionSide, directionToRegion);

            if (neighborRegion.Options.Count == 1)
            {
                CollapseRegionTo(neighborPosition, neighborRegion.Options[0]);
            }
        }

        return this[regionPosition];
    }

    public FlatArray<char> ToFlatArray()
    {
        var flatArray = new FlatArray<char>(_regionsWidth * _regionSize, _regionsHeight * _regionSize);
        for (int x = 0; x < flatArray.Width; x++)
        {
            for (int y = 0; y < flatArray.Height; y++)
            {
                var regionX = x / _regionSize;
                var regionY = y / _regionSize;
                var currentRegion = _regionsMap[new Point(regionX, regionY)];

                var cellX = x - (currentRegion.GridCoordinates.X * _regionSize);
                var cellY = y - (currentRegion.GridCoordinates.Y * _regionSize);
                flatArray[x, y] = currentRegion.Map[cellX, cellY];
            }
        }

        return flatArray;
    }

    public IEnumerator<RegionData> GetEnumerator()
    {
        return ((IEnumerable<RegionData>)_regionArray).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _regionArray.GetEnumerator();
    }

    private ValidationResult ValidateCells()
    {
        foreach (var region in this)
        {
            foreach (var cell in region.Map)
            {
                // TODO
                if (cell == '\0')
                {
                    return new ValidationResult(false, $"Region [{region.GridCoordinates}]:[{region.Id}] contains cell with invalid glyphs: [{cell}].");
                }
            }
        }

        return ValidationResult.Success;
    }

    private List<Ulid> GetValidRegions(List<Ulid> inputMap, char[] side, Direction.Types dir)
    {
        var result = new List<Ulid>();
        foreach (var inputId in inputMap)
        {
            if (_inputRegionMap[inputId].Sides[dir].SequenceEqual(side))
            {
                result.Add(inputId);
            }
        }
        return result;
    }

    private static List<RegionData> SplitToRegions(FlatArray<char> map, int regionSize)
    {
        var columns = map.Width / regionSize;
        var rows = map.Height / regionSize;
        var result = new List<RegionData>();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // x and y are cell-region coordinates, where the region is a square with _cellSize sides
                var regionMap = new FlatArray<char>(regionSize, regionSize);
                var cellXStart = x * regionSize;
                var cellYStart = y * regionSize;

                for (int cellX = cellXStart; cellX < cellXStart + regionSize; cellX++)
                {
                    for (int cellY = cellYStart; cellY < cellYStart + regionSize; cellY++)
                    {
                        // cellX and cellY are actual cell coordinates in the big flatArray
                        regionMap[cellX - cellXStart, cellY - cellYStart] = map[cellX, cellY];
                    }
                }

                result.Add(new RegionData(regionMap, new Point(x, y)));
            }
        }

        return result;
    }
}

