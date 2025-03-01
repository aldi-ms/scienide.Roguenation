namespace scienide.Engine.Game.Pathfinding;

using Newtonsoft.Json;
using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Infrastructure;

public static class NeighbourCache
{
    private static readonly Dictionary<Point, Point[]> _mapNeighbours = [];

    public static Dictionary<Point, Point[]> MapNeighbours => _mapNeighbours;

    public static void InitMapNeighbours(FlatArray<Cell> map)
    {
        var neighborCells = new Point[8];
        foreach (var cell in map)
        {
            if (cell.Properties[Props.IsWalkable])
            {
                var neighbours = GetValidWalkableNeighbours(cell, map, neighborCells);
                _mapNeighbours.Add(cell.Position, neighbours);
            }
        }
    }

    public static void DumpNeighbourCache()
    {
        var json = JsonConvert.SerializeObject(_mapNeighbours, Formatting.Indented);
        File.WriteAllText(@".\PathfindingNeighbour.dump.txt", json);
    }

    private static Point[] GetValidWalkableNeighbours(Cell cell, FlatArray<Cell> map, Point[] neighborArr)
    {
        int neighborCount = 0;
        for (int dX = -1; dX <= 1; dX++)
        {
            for (int dY = -1; dY <= 1; dY++)
            {
                if (dX == 0 && dY == 0)
                    continue;

                var x = cell.Position.X + dX;
                var y = cell.Position.Y + dY;

                if (x < 0 || x >= map.Width || y < 0 || y >= map.Height)
                {
                    continue;
                }

                if (!map[x, y].Properties[Props.IsWalkable])
                {
                    continue;
                }
                neighborArr[neighborCount++] = new Point(x, y);
            }
        }

        var resultArray = new Point[neighborCount];
        Array.Copy(neighborArr, resultArray, neighborCount);
        return resultArray;
    }

    //private class PathDictionaryConverter(JsonTypeInfo<Point[]> pointArrayTypeInfo) : JsonConverter<Dictionary<Point, Point[]>>
    //{
    //    private readonly JsonTypeInfo<Point[]> _pointArrayTypeInfo = pointArrayTypeInfo;

    //    public override Dictionary<Point, Point[]> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        var dict = new Dictionary<Point, Point[]>();
    //        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
    //        {
    //            foreach (var property in doc.RootElement.EnumerateObject())
    //            {
    //                string[] coords = property.Name.Split(',');
    //                var key = new Point(int.Parse(coords[0]), int.Parse(coords[1]));
    //                var value = JsonSerializer.Deserialize(property.Value, _pointArrayTypeInfo);
    //                dict[key] = value!;
    //            }
    //        }
    //        return dict;
    //    }

    //    public override void Write(Utf8JsonWriter writer, Dictionary<Point, Point[]> value, JsonSerializerOptions options)
    //    {
    //        writer.WriteStartObject();
    //        foreach (var kvp in value)
    //        {
    //            string key = $"{kvp.Key.X},{kvp.Key.Y}";
    //            writer.WritePropertyName(key);
    //            JsonSerializer.Serialize(writer, kvp.Value, _pointArrayTypeInfo);
    //        }
    //        writer.WriteEndObject();
    //    }
    //}
}
