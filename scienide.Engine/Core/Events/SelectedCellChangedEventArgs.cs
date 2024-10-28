namespace scienide.Engine.Core.Events;

using scienide.Engine.Game;

public class SelectedCellChangedEventArgs(Cell cell) : EventArgs
{
    public Cell SelectedCell { get; set; } = cell;
}
