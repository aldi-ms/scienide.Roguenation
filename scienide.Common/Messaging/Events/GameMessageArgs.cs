namespace scienide.Common.Messaging.Events;

using SadRogue.Primitives;

public class GameMessageArgs(Point source, string message, ushort intensity) : EventArgs
{
    public ushort Intensity { get; } = intensity;
    public string Message { get; } = message;
    public Point Source { get; } = source;
}
