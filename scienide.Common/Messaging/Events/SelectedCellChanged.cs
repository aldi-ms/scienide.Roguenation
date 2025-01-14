namespace scienide.Common.Messaging.Events;

using scienide.Common.Game;

public class SelectedCellChanged(Cell cell) : BaseMessageEvent
{
    public Cell SelectedCell { get; set; } = cell;
}
