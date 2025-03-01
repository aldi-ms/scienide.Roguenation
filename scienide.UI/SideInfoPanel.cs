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

public class SideInfoPanel : IDisposable
{
    private const ushort HeroUIUpdateInterval = 200; // milliseconds
    private const string GrayOneCharOutLine = "[c:r f:slategray:1]";
    private const string GrayDescriptionLine = "[c:r f:lightgray]";

    private readonly Console _console;
    private readonly Rectangle _topRect;
    private readonly Rectangle _midRect;
    private readonly Rectangle _botRect;
    private readonly IActor? _hero = null;
    private readonly System.Timers.Timer _timer;

    private int _panesFilled = 0;
    private Cell? _selectedCell = null;
    private Ulid _selectedActorId = Ulid.Empty;
    private bool _disposedValue;

    public SideInfoPanel(ICellSurface surface, IActor hero)
    {
        _console = new Console(surface);
        _console.Cursor.PrintAppearanceMatchesHost = false;
        var rectHeight = _console.Height / 3;
        _topRect = new Rectangle(Point.Zero, new Point(_console.Width - 1, rectHeight));
        _midRect = new Rectangle(new Point(0, rectHeight + 1), new Point(_console.Width - 1, rectHeight * 2));
        _botRect = new Rectangle(new Point(0, (rectHeight * 2) + 1), new Point(_console.Width - 1, rectHeight * 3));
        _hero = hero;

        MessageBroker.Instance.Subscribe<SelectedCellChanged>(SelectedCellChangedHandler);
        MessageBroker.Instance.Subscribe<ActorDeathMessage>(ActorDeathMsgHandler);

        _timer = new(TimeSpan.FromMilliseconds(HeroUIUpdateInterval));
        _timer.Elapsed += Timer_Elapsed;
        _timer.Start();
    }

    private void ActorDeathMsgHandler(ActorDeathMessage message)
    {
        if (message.Actor.Id == _selectedActorId)
        {
            _console.Clear(_midRect);
            _console.DrawLine((0, _midRect.Height + _midRect.Y - 1), (_midRect.Width, _midRect.Height + _midRect.Y - 1), 196, Color.White);
        }
        else if (message.Actor == _hero)
        {
            _timer.Stop();
            _console.Clear(_topRect);

            _console.Cursor
                .Move(3, 3)
                .Print(Global.StringParser.Parse("[c:r f:red]GAME OVER!!!"));
        }
    }

    private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        RedrawActorScreen();
    }

    private void RedrawActorScreen()
    {
        ArgumentNullException.ThrowIfNull(_hero);

        if (!_hero.TryGetComponent<StatsComponent>(out var actorStats, true))
        {
            return;
        }

        _console.Clear(_topRect);
        _console.DrawLine((0, _topRect.Height - 1), (_topRect.Width, _topRect.Height - 1), 196, Color.White);
        _console.Cursor
            .Move(1, _topRect.Y)
            .Print(Global.StringParser.Parse($"{GrayOneCharOutLine}[ {_hero.Name}: "))
            .Move(_console.Cursor.Position.X + 1, _topRect.Y)
            .Print(_hero.Glyph.ToString(), _hero.Glyph.Appearance, null)
            .Print(Global.StringParser.Parse($" {GrayOneCharOutLine}]"))
            .Move(0, _topRect.Y + 1)
            .Print($"HP: {actorStats.CurrentHealth} / {actorStats.MaxHealth}");

        CreateHPBar(_topRect.Width, 1, actorStats);

        _console.Cursor
            .Move(1, 2)
            .Print(Global.StringParser.Parse($"{GrayDescriptionLine}at {_hero.Position}"));
    }

    private void RedrawSelectedCell()
    {
        ArgumentNullException.ThrowIfNull(_selectedCell);
        _panesFilled = 0;

        if (_selectedCell.Actor != null)
        {
            _panesFilled++;
            _console.Clear(_midRect);
            _console.DrawLine((0, _midRect.Height + _midRect.Y - 1), (_midRect.Width, _midRect.Height + _midRect.Y - 1), 196, Color.White);

            _console.Cursor
                .Move(_midRect.X + 1, _midRect.Y)
                .Print(Global.StringParser.Parse($"{GrayOneCharOutLine}[ {_selectedCell.Actor.Name}: "))
                .Move(_console.Cursor.Position.X + 1, _midRect.Y)
                .Print(_selectedCell.Actor.Glyph.ToString(), _selectedCell.Actor.Glyph.Appearance, null)
                .Print(Global.StringParser.Parse($" {GrayOneCharOutLine}]"))
                .Move(2, _midRect.Y + 1)
                .Print(Global.StringParser.Parse($"{GrayDescriptionLine}at {_selectedCell.Actor.Position}"));

            if (_selectedCell.Actor.TryGetComponent<StatsComponent>(out var actorStats, true))
            {
                _console.Cursor
                    .Move(1, _midRect.Y + 2)
                    .Print($"HP: {actorStats.CurrentHealth} / {actorStats.MaxHealth}");
                CreateHPBar(_midRect.Width, _midRect.Y + 2, actorStats);
            }

            var componentInfo = _selectedCell.Actor.FetchComponentShortData();
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
            .Print(_selectedCell.Terrain.Glyph.ToString(), _selectedCell.Terrain.Glyph.Appearance, null)
            .Move(terrainRect.X + 2, _console.Cursor.Position.Y + 1)
            .Print(Global.StringParser.Parse($"{GrayDescriptionLine}at {_selectedCell.Position}:"));

        var dRow = 3;
        foreach (var prop in Enum.GetValues<Props>())
        {
            if (_selectedCell.Properties[prop])
            {
                _console.Cursor
                    .Move(terrainRect.Position.Add(new Point(1, dRow++)))
                    .Print($"{prop}");
            }
        }
    }

    private void SelectedCellChangedHandler(SelectedCellChanged args)
    {
        _selectedCell = args.SelectedCell;
        if (_selectedCell.Actor != null)
        {
            _selectedActorId = _selectedCell.Actor.Id;
        }

        RedrawSelectedCell();
    }

    private void CreateHPBar(int width, int row, StatsComponent actorStats)
    {
        var hpPercent = actorStats.CurrentHealth / (float)actorStats.MaxHealth;
        var barColor = hpPercent switch
        {
            > .75f => Color.Green,
            > .45f => Color.Goldenrod,
            > .25f => Color.DarkRed,
            _ => Color.Red
        };
        var barWidth = width * hpPercent;

        for (var i = 0; i < barWidth; i++)
        {
            _console.Surface.SetBackground(i, row, barColor);
        }
    }

    protected void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _timer.Dispose();
                _console.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
