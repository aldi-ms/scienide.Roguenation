namespace scienide.Common.Map;

using scienide.Common.Game;
using System.Collections.Generic;

public class RegionData(HashSet<Cell> cells, HashSet<Cell> walls)
{
    public HashSet<Cell> Cells { get; set; } = cells;
    public HashSet<Cell> Walls { get; set; } = walls;
}