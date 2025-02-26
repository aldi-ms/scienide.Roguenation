namespace scienide.Common.Messaging;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

public abstract class BaseMessageEvent : EventArgs
{
    public bool Consume { get; set; } = false;
}

public class SystemMessage(string message, string? source = null) : BaseMessageEvent
{
    public string? Source { get; } = source;
    public string Message { get; } = message;
}

public class SelectedCellChanged(Cell cell) : BaseMessageEvent
{
    public Cell SelectedCell { get; set; } = cell;
}

public class GameMessage(Point source, string message, ushort intensity) : BaseMessageEvent
{
    public ushort Intensity { get; } = intensity;
    public string Message { get; } = message;
    public Point Source { get; } = source;
}

public class ActorDeathMessage : BaseMessageEvent
{
    public Ulid ActorId { get; }
    public IActor Actor { get; }

    public ActorDeathMessage(IActor actor)
    {
        Actor = actor;
        Consume = true;
    }
}