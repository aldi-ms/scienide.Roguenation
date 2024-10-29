namespace scienide.UI;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Messaging.Events;
using scienide.Engine.Core.Messaging;

public class InfoPanel
{
    private const string UnitTitleStyle = "[c:r f:gold]";
    private const string GrayOneCharOutLine = "[c:r f:slategray:1]";
    private const string GrayDescriptionLine = "[c:r f:lightgray]";
    private readonly Console _console;
    private readonly Rectangle _actorRect;

    public InfoPanel(ICellSurface surface)
    {
        _console = new Console(surface);
        _actorRect = new Rectangle(Point.Zero, new Point(_console.Width - 1, 10));
        MessageBroker.Instance.Subscribe<SelectedCellChangedEventArgs>(SelectedCellChanged);
    }

    private void SelectedCellChanged(SelectedCellChangedEventArgs args)
    {
        var cell = args.SelectedCell;
        if (cell.Actor != null)
        {
            _console.Clear(_actorRect);
            _console.DrawBox(_actorRect, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, new ColoredGlyph(Color.Gray, Color.Black, '=')));
            var actorTitle = $"{GrayOneCharOutLine}[{cell.Actor.Name}{GrayOneCharOutLine}]";
            var sideAccentCharCount = (_actorRect.Width - 2 - (cell.Actor.Name.Length + 2)) / 2;
            var sideAccentString = new string('=', sideAccentCharCount);
            var headline = $"{UnitTitleStyle}{sideAccentString}{actorTitle}{sideAccentString}";
            _console.Cursor.Move(1, 1).Print(Global.StringParser.Parse(headline));
            _console.Cursor.Move(1, 3).Print(Global.StringParser.Parse($"{GrayDescriptionLine}Position: {cell.Actor.Position}"));
        }
    }
}
