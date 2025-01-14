namespace scienide.Common.Messaging.Events;

using SadRogue.Primitives;

public class GameMessage(Point source, string message, ushort intensity) : BaseMessageEvent
{
    public ushort Intensity { get; } = intensity;
    public string Message { get; } = message;
    public Point Source { get; } = source;
}
