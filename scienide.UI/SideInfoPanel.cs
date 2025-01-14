namespace scienide.UI;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Infrastructure;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using Console = SadConsole.Console;

public class SideInfoPanel
{
    private const string GrayOneCharOutLine = "[c:r f:slategray:1]";
    private const string GrayDescriptionLine = "[c:r f:lightgray]";
    private int _panesFilled = 0;
    private readonly Console _console;
    private readonly Rectangle _topRect;
    private readonly Rectangle _midRect;

    public SideInfoPanel(ICellSurface surface)
    {
        _console = new Console(surface);
        _console.Cursor.PrintAppearanceMatchesHost = false;
        _topRect = new Rectangle(Point.Zero, new Point(_console.Width - 1, 10));
        _midRect = new Rectangle(new Point(0, 11), new Point(_console.Width - 1, 20));
        //_botRect = new Rectangle(new Point(0, 21), new Point(_console.Width - 1, 30));

        MessageBroker.Instance.Subscribe<SelectedCellChanged>(SelectedCellChanged);
    }

    private void SelectedCellChanged(SelectedCellChanged args)
    {
        _console.Clear();
        _panesFilled = 0;
        var cell = args.SelectedCell;
        if (cell.Actor != null)
        {
            _panesFilled++;
            _console.DrawBox(_topRect, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, borderColors: new ColoredGlyph(Color.Gray, Color.Black, '=')));

            _console.Cursor
                .Move(1, 1)
                .Print(Global.StringParser.Parse($"{GrayOneCharOutLine}[ {cell.Actor.Name}: "))
                .Move(_console.Cursor.Position.X + 1, _console.Cursor.Position.Y)
                .Print(cell.Actor.Glyph.ToString(), cell.Actor.Glyph.Appearance, null)
                .Print(Global.StringParser.Parse($" {GrayOneCharOutLine}]"));
            _console.Cursor
                .Move(2, 2)
                .Print(Global.StringParser.Parse($"{GrayDescriptionLine}at {cell.Actor.Position}"));

            var componentInfo = cell.Actor.FetchComponentStatuses();
            if (componentInfo.Count > 0)
            {
                var dy = 3;
                foreach (var info in componentInfo)
                {
                    _console.Cursor
                        .Move(1, dy++)
                        .Print($"{info.Key[0]}: {info.Value}");
                }
            }
        }

        var terrainRect = _panesFilled == 0 ? _topRect : _midRect;
        _console.DrawBox(terrainRect, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, borderColors: new ColoredGlyph(Color.Gray, Color.Black, '=')));

        _console.Cursor
            .Move(terrainRect.Position + 1)
            .Print($"Terrain: ")
            .Move(_console.Cursor.Position.X + 1, _console.Cursor.Position.Y)
            .Print(cell.Terrain.Glyph.ToString(), cell.Terrain.Glyph.Appearance, null);
        _console.Cursor
            .Move(terrainRect.Position + 2)
            .Print(Global.StringParser.Parse($"{GrayDescriptionLine}at {cell.Position}:"));

        var dRow = 3;

        foreach (var prop in Enum.GetValues<Props>())
        {
            if (cell.Properties[prop])
            {
                _console.Cursor
                    .Move(terrainRect.Position.Add(new Point(1, dRow++)))
                    .Print($"{prop}");
            }
        }
    }
}
