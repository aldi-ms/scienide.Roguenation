namespace scienide.UI;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using Console = SadConsole.Console;

public class SideInfoPanel
{
    private const string UnitTitleStyle = "[c:r f:gold]";
    private const string GrayOneCharOutLine = "[c:r f:slategray:1]";
    private const string GrayDescriptionLine = "[c:r f:lightgray]";
    private const string GlyphEmphasis = "[c:r f:white:1]";
    private int _panesFilled = 0;
    private readonly Console _console;
    private readonly Rectangle _topRect;
    private readonly Rectangle _midRect;
    private readonly Rectangle _botRect;

    public SideInfoPanel(ICellSurface surface)
    {
        _console = new Console(surface);
        _topRect = new Rectangle(Point.Zero, new Point(_console.Width - 1, 10));
        _midRect = new Rectangle(new Point(0, 11), new Point(_console.Width - 1, 20));
        _botRect = new Rectangle(new Point(0, 21), new Point(_console.Width - 1, 30));

        MessageBroker.Instance.Subscribe<SelectedCellChangedArgs>(SelectedCellChanged);
    }

    private void SelectedCellChanged(SelectedCellChangedArgs args)
    {
        _console.Clear();
        var cell = args.SelectedCell;
        if (cell.Actor != null)
        {
            _panesFilled++;
            _console.DrawBox(_topRect, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, borderColors: new ColoredGlyph(Color.Gray, Color.Black, '=')));

            var actorTitle = $"{GrayOneCharOutLine}[ {cell.Actor.Name}: {GlyphEmphasis}{cell.Actor.Glyph} {GrayOneCharOutLine}]";
            var sideAccentString = string.Empty;
            var headline = $"{UnitTitleStyle}{sideAccentString}{actorTitle}{sideAccentString}";

            _console.Cursor.Move(1, 1).Print(Global.StringParser.Parse(headline));
            _console.Cursor.Move(1, 3).Print(Global.StringParser.Parse($"{GrayDescriptionLine}Position: {cell.Actor.Position}"));
            _console.Print(1, 4, cell.Actor.Glyph.ToString(), cell.Actor.Glyph.Appearance);
        }

        var terrainRect = _panesFilled == 0 ? _topRect : _midRect;
        _console.DrawBox(terrainRect, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, borderColors: new ColoredGlyph(Color.Gray, Color.Black, '=')));
        _console.Cursor.Move(terrainRect.Position + 1).Print($"at {cell.Position}:");
        _console.Cursor.Move(terrainRect.Position + 2).Print($"Terrain: {cell.Terrain.Glyph}");
    }
}
