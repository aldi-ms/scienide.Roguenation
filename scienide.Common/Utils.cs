namespace scienide.Common;

using SadRogue.Primitives;

public static class Utils
{
    public static Direction GetRandomValidDirection()
    {
        int dX, dY;
        do
        {
            dX = Global.RNG.Next(-1, 2);
            dY = Global.RNG.Next(-1, 2);
        }
        while (dX == 0 && dY == 0);

        return Direction.GetDirection(dX, dY);
    }

    public static float ManhattanDistance(Point point1, Point point2)
    {
        return MathF.Abs(point1.X - point2.X) + MathF.Abs(point1.Y - point2.Y);
    }

    public static float EuclideanDistance(Point point1, Point point2)
    {
        return MathF.Sqrt(((point1.X - point2.X) * (point1.X - point2.X)) + ((point1.Y - point2.Y) * (point1.Y - point2.Y)));
    }

    public static void WriteToFile(string fName, string data)
    {
        if (string.IsNullOrWhiteSpace(fName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fName));
        }

        if (data is null)
        {
            throw new ArgumentNullException(nameof(data), "Data cannot be null.");
        }

        var dirName = Path.GetDirectoryName(fName);
        if (dirName != null && !Directory.Exists(dirName))
        {
            Directory.CreateDirectory(dirName);
        }

        try
        {
            File.WriteAllText(fName, data);
        }
        catch (Exception ex)
        {
            throw new IOException($"An error occurred while writing to the file: {fName}", ex);
        }
    }
}
