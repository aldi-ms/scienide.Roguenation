namespace scienide.UI;

using SadConsole;
using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Components;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;
using scienide.Common.Messaging;
using Console = SadConsole.Console;

public class SideInfoPanel
{
    private const string GrayOneCharOutLine = "[c:r f:slategray:1]";
    private const string GrayDescriptionLine = "[c:r f:lightgray]";

    private readonly Console _console;
    private readonly Rectangle _topRect;
    private readonly Rectangle _midRect;
    private readonly Rectangle _botRect;
    private readonly IActor? _actor = null;
    private readonly System.Timers.Timer _timer;
    
    private int _panesFilled = 0;
    private Cell? _cell = null;

    public SideInfoPanel(ICellSurface surface, IActor hero)
    {
        _console = new Console(surface);
        _console.Cursor.PrintAppearanceMatchesHost = false;
        _topRect = new Rectangle(Point.Zero, new Point(_console.Width - 1, 10));
        _midRect = new Rectangle(new Point(0, 11), new Point(_console.Width - 1, 20));
        _botRect = new Rectangle(new Point(0, 21), new Point(_console.Width - 1, 30));
        _actor = hero;

        MessageBroker.Instance.Subscribe<SelectedCellChanged>(SelectedCellChanged);
        _timer = new(TimeSpan.FromMilliseconds(300));
        _timer.Elapsed += Timer_Elapsed;
        _timer.Start();
    }

    private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        RedrawActorScreen();
    }

    private void RedrawActorScreen()
    {
        ArgumentNullException.ThrowIfNull(_actor);

        _console.Clear(_topRect);
        if (!_actor.TryGetComponent<StatsComponent>(out var actorStats, true))
        {
            return;
        }

        _console.Cursor
            .Move(1, _topRect.Y)
            .Print(Global.StringParser.Parse($"{GrayOneCharOutLine}[ {_actor.Name}: "))
            .Move(_console.Cursor.Position.X + 1, _topRect.Y)
            .Print(_actor.Glyph.ToString(), _actor.Glyph.Appearance, null)
            .Print(Global.StringParser.Parse($" {GrayOneCharOutLine}]"))
            .Move(0, _topRect.Y + 1)
            .Print($"HP: {actorStats.CurrentHealth} / {actorStats.MaxHealth}");

        var hpPercent = actorStats.CurrentHealth / (float)actorStats.MaxHealth;
        var barColor = hpPercent switch
        {
            > .75f => Color.Green,
            > .45f => Color.Goldenrod,
            > .25f => Color.DarkRed,
            _ => Color.Red
        };
        var barWidth = _topRect.Width * hpPercent;

        for (var i = 0; i < barWidth; i++)
        {
            _console.Surface.SetBackground(i, 1, barColor);
        }

        _console.DrawLine((0, _topRect.Height - 1), (_topRect.Width, _topRect.Height - 1), 196, Color.White);
    }

    private void RedrawSelectedCell()
    {
        if (_cell == null) return;

        _panesFilled = 0;
        var cell = _cell;
        if (cell.Actor != null)
        {
            _panesFilled++;
            _console.Clear(_midRect);
            _console.DrawLine((0, _midRect.Height + _midRect.Y - 1), (_midRect.Width, _midRect.Height + _midRect.Y - 1), 196, Color.White);

            _console.Cursor
                .Move(_midRect.X + 1, _midRect.Y)
                .Print(Global.StringParser.Parse($"{GrayOneCharOutLine}[ {cell.Actor.Name}: "))
                .Move(_console.Cursor.Position.X + 1, _midRect.Y)
                .Print(cell.Actor.Glyph.ToString(), cell.Actor.Glyph.Appearance, null)
                .Print(Global.StringParser.Parse($" {GrayOneCharOutLine}]"))
                .Move(2, _midRect.Y + 1)
                .Print(Global.StringParser.Parse($"{GrayDescriptionLine}at {cell.Actor.Position}"));

            if (cell.Actor.TryGetComponent<StatsComponent>(out var actorStats, true))
            {
                _console.Cursor
                    .Move(1, _midRect.Y + 2)
                    .Print($"HP: {actorStats.CurrentHealth} / {actorStats.MaxHealth}");
            }

            var componentInfo = cell.Actor.FetchComponentStatuses();
            if (componentInfo.Count > 0)
            {
                var dy = 3;
                foreach (var info in componentInfo)
                {
                    _console.Cursor
                        .Move(1, _midRect.Position.Y + dy++)
                        .Print($"{info.Key[0]}: {info.Value}");
                }
            }
        }

        var terrainRect = _panesFilled == 0 ? _midRect : _botRect;

        _console.Clear(terrainRect);

        _console.Cursor
            .Move(terrainRect.X + 1, terrainRect.Y)
            .Print($"Terrain: ")
            .Move(_console.Cursor.Position.X + 1, _console.Cursor.Position.Y)
            .Print(cell.Terrain.Glyph.ToString(), cell.Terrain.Glyph.Appearance, null);
        _console.Cursor
            .Move(terrainRect.X + 2, _console.Cursor.Position.Y + 1)
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

    private void SelectedCellChanged(SelectedCellChanged args)
    {
        _cell = args.SelectedCell;
        RedrawSelectedCell();
    }
}
