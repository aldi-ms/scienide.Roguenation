namespace scienide.Common.Map;

using scienide.Common.Game;
using System.Collections.Generic;

public class RegionCellData(HashSet<Cell> cells, HashSet<Cell> walls)
{
    public Ulid Id { get; } = Ulid.NewUlid();
    public HashSet<Cell> Cells { get; set; } = cells;
    public HashSet<Cell> Borders { get; set; } = walls;
}