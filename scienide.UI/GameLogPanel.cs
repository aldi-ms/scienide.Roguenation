namespace scienide.UI;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using System.Diagnostics;

public class GameLogPanel
{
    private const string GlobalMessageStyle = "[c:r f:slategray]";
    private const string SystemMessageStyle = "[c:r f:gold]";
    private int _current;
    private readonly int _lineCount;
    private readonly Console _console;
    private readonly Point[] _linePositions;
    private readonly string[] _lines;

    public GameLogPanel(ICellSurface surface, int numberOfLines, IMessageSubscriber sub)
    {
        _lineCount = numberOfLines;
        _console = new Console(surface);
        _lines = new string[numberOfLines];
        _linePositions = new Point[numberOfLines];

        for (int i = 0; i < numberOfLines; i++)
        {
            _linePositions[i] = new Point(0, i);
        }

        AddMessage(GlobalMessageStyle + $"Game ver. 0.01a running with seed [{Global.Seed}].");
        MessageBroker.Instance.Subscribe<GameMessageArgs>(GameMessageListener, sub);
        MessageBroker.Instance.Subscribe<SystemMessageArgs>(SystemMessageReceived);
    }

    private void SystemMessageReceived(SystemMessageArgs args)
    {
        AddMessage(SystemMessageStyle + $"{(string.IsNullOrWhiteSpace(args.Source) ? string.Empty : $"[{args.Source}]: ")}" + args.Message);
    }

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
            if (string.IsNullOrWhiteSpace(_lines[i]))
            {
                continue;
            }

            _console.Cursor.Move(_linePositions[i]).Print(Global.StringParser.Parse(_lines[i])).NewLine();
        }
    }

    public void ClearScreen()
    {
        _console.Clear();
        _console.Cursor.Position = Point.Zero;
    }
}
