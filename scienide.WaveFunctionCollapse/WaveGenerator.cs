namespace scienide.WaveFunctionCollapse;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Infrastructure;
using System.Diagnostics;

public class WaveGenerator
{
    private const int MAX_RUNS = 15;

    private readonly RegionMap _inputRegionMap;

    public int MapSizeX { get; set; }
    public int MapSizeY { get; set; }
    public int RegionSize { get; set; }

    public WaveGenerator(int mapRegionSizeX, int mapRegionSizeY, int regionSize, string inputFile)
    {
        MapSizeX = mapRegionSizeX;
        MapSizeY = mapRegionSizeY;
        RegionSize = regionSize;

        var input = InputParser.ReadInputFile(inputFile);
        _inputRegionMap = new RegionMap(input, RegionSize);
    }


    public FlatArray<char> Run()
    {
        var run = 0;
        while (run < MAX_RUNS)
        {
            var outputMap = new FlatArray<char>(MapSizeX, MapSizeY);
            var outputRegions = new RegionMap(outputMap, RegionSize);
            outputRegions.Initialize(_inputRegionMap);

            // Set random region to have a starting point
            var randomRegion = _inputRegionMap.ElementAt(Global.RNG.Next(_inputRegionMap.Count));
            var randomStartPoint = GetRandomRegionStartPoint(outputMap);
            _ = outputRegions.CollapseRegion(randomStartPoint, randomRegion.Id);

            // Calculate collapse variants
            var result = CollapseFunction(outputRegions);
            if (!result.IsValid)
            {
                run++;
                Trace.WriteLine($"Map generated is invalid with msg: {result.Message}");
            }
            else
            {
                return outputRegions.ToFlatArray();
            }
        }

        throw new ArgumentException("Could not generate map with given input!");
    }

    private static ValidationResult CollapseFunction(RegionMap outputRegions)
    {
        var sortedRegions = outputRegions
            .Where(x => !x.IsCollapsed && x.Options.Count > 0)
            .GroupBy(x => x.Options.Count)
            .OrderBy(x => x.Key)
            .FirstOrDefault()
            ?.ToList();

        while (sortedRegions != null)
        {
            var regionSelected = sortedRegions[Global.RNG.Next(sortedRegions.Count)];
            var optionSelected = regionSelected.Options[Global.RNG.Next(regionSelected.Options.Count)];
            outputRegions.CollapseRegion(regionSelected.GridCoordinates, optionSelected);

            sortedRegions = outputRegions
                .Where(x => !x.IsCollapsed && x.Options.Count > 0)
                .GroupBy(x => x.Options.Count)
                .OrderBy(x => x.Key)
                .FirstOrDefault()
                ?.ToList();
        }

        return outputRegions.IsValidMap();
    }

    private Point GetRandomRegionStartPoint(FlatArray<char> map)
    {
        return new Point(
            Global.RNG.Next(map.Width / RegionSize),
            Global.RNG.Next(map.Height / RegionSize));
    }
}