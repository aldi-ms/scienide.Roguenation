namespace scienide.UI;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using System;
using Console = SadConsole.Console;

public class InfoPanel
{
    private const string UnitTitleStyle = "[c:r f:gold]";
    private const string GrayOneCharOutLine = "[c:r f:slategray:1]";
    private const string GrayDescriptionLine = "[c:r f:lightgray]";
    private const string GlyphEmphasis = "[c:r f:white:1]";
    private readonly Console _console;
    private readonly Rectangle _topRect;
    private readonly Rectangle _midRect;
    private readonly Rectangle _botRect;

    public InfoPanel(ICellSurface surface)
    {
        _console = new Console(surface);
        _topRect = new Rectangle(Point.Zero, new Point(_console.Width - 1, 10));
        _midRect = new Rectangle(new Point(0, 11), new Point(_console.Width - 1, 20));
        _botRect = new Rectangle(new Point(0, 21), new Point(_console.Width - 1, 30));

        MessageBroker.Instance.Subscribe<SelectedCellChangedEventArgs>(SelectedCellChanged);
    }

    private void SelectedCellChanged(SelectedCellChangedEventArgs args)
    {
        _console.Clear();
        int panesFilled = 0;
        var cell = args.SelectedCell;
        if (cell.Actor != null)
        {
            panesFilled++;
            _console.DrawBox(_topRect, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, borderColors: new ColoredGlyph(Color.Gray, Color.Black, '=')));

            var actorTitle = $"{GrayOneCharOutLine}[ {cell.Actor.Name}: {GlyphEmphasis}{cell.Actor.Glyph} {GrayOneCharOutLine}]";
            //var sideAccentCharCount = (_actorRect.Width - 5 - (cell.Actor.Name.Length + 2)) / 2;
            //var sideAccentString = new string(' ', sideAccentCharCount);
            var sideAccentString = string.Empty;
            var headline = $"{UnitTitleStyle}{sideAccentString}{actorTitle}{sideAccentString}";

            _console.Cursor.Move(1, 1).Print(Global.StringParser.Parse(headline));
            _console.Cursor.Move(1, 3).Print(Global.StringParser.Parse($"{GrayDescriptionLine}Position: {cell.Actor.Position}"));
        }

        var terrainRect = panesFilled == 0 ? _topRect : _midRect;
        _console.DrawBox(terrainRect, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, borderColors: new ColoredGlyph(Color.Gray, Color.Black, '=')));
        _console.Cursor.Move(terrainRect.Position + 1).Print($"at {cell.Position}:");
        _console.Cursor.Move(terrainRect.Position + 2).Print($"Terrain: {cell.Terrain.Glyph}");
    }
}
