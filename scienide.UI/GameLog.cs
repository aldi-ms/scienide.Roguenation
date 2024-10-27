namespace scienide.UI;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using scienide.Engine.Core.Messaging;
using System.Diagnostics;

public class GameLog
{
    private int _current;
    private int _lineCount;
    private Console _console;
    private Point[] _linePositions;
    private string[] _lines;

    public GameLog(ICellSurface surface, int numberOfLines, IMessageSubscriber sub)
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
        MessageBroker.Instance.Subscribe<GameMessageArgs>(GameMessageListener, sub);
    }

    public Console Console => _console;

    public void GameMessageListener(GameMessageArgs args)
    {
        AddMessage(args.Message);
    }

    public void AddMessage(string message)
    {
        Trace.WriteLine($"[{nameof(AddMessage)}]: {message}");
        _lines[_current++] = message;
        ClearScreen();
        DrawCurrentLines();

        if (_current >= _lineCount)
        {
            // Start from index 1, since the last (first added) message should be discarded
            for (int i = 1; i < _lineCount; i++)
            {
                _lines[i - 1] = _lines[i];
            }

            _current = _lineCount - 1;
        }
    }

    public void DrawCurrentLines()
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
