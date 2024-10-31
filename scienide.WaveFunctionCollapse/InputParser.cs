namespace scienide.WaveFunctionCollapse;

using scienide.Common.Infrastructure;

internal static class InputParser
{
    public static FlatArray<char> ReadInputFile(string fileName)
    {
        var flatArray = new FlatArray<char>(0, 0);
        using var fs = File.OpenRead(fileName);
        using var reader = new StreamReader(fs);

        int lineNumber = 0;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new ArgumentNullException(nameof(fileName));

            if (lineNumber == 0)
            {
                // First iteration set the array with the size of the line
                // we expect a square region of input
                flatArray = new FlatArray<char>(line.Length, line.Length);
            }

            for (int i = 0; i < line.Length; i++)
            {
                flatArray[i, lineNumber] = line[i];
            }

            lineNumber++;
        }

        return flatArray;
    }
}
