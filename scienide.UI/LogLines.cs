namespace scienide.UI;

using SadConsole;
using SadRogue.Primitives;
using System.Diagnostics;

public class LogLines
{
    private int _current;
    private int _lineCount;
    private Console _console;
    private Point[] _linePositions;
    private string[] _lines;

    public LogLines(ICellSurface surface, int numberOfLines)
    {
        _lineCount = numberOfLines;
        _console = new Console(surface);
        _lines = new string[numberOfLines];
        _linePositions = new Point[numberOfLines];

        for (int i = 0; i < numberOfLines; i++)
        {
            _linePositions[i] = new Point(0, i);
        }

        AddMessage("Game ver. 0.01a running.");
    }

    public Console Console => _console;

    public void AddMessage(string message)
    {
        Trace.WriteLine($"[{nameof(AddMessage)}]: {message}");

        ClearScreen();
        DrawCurrent();

        //_lines[_current] = message;
        //_console.Cursor.Move(_linePositions[_current]).Print(message.PadRight(_console.Width, ' ')).NewLine();
        
        _current++;
        
        if (_current >= _lineCount)
        {
            // Start from index 1, since the last (first added) message should be discarded
            for (int i = 1; i < _lineCount; i++)
            {
                _lines[i - 1] = _lines[i];
            }

            _current = _lineCount - 1;
            //ClearScreen();
            //DrawCurrent();
        }
    }

    public void DrawCurrent()
    {
        for (int i = 0; i < _lineCount; i++)
        {
            _console.Cursor.Move(_linePositions[i]).Print(_lines[i]).NewLine();
        }
    }

    public void ClearScreen()
    {
        _console.Clear();
        _console.Cursor.Position = Point.Zero;
    }
}
