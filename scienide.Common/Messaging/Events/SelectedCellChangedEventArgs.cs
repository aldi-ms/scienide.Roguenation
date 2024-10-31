﻿namespace scienide.Common.Messaging.Events;

using scienide.Common.Game;

public class SelectedCellChangedEventArgs(Cell cell) : EventArgs
{
    public Cell SelectedCell { get; set; } = cell;
}