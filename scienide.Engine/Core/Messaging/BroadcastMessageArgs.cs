namespace scienide.Engine.Core.Messaging;

using SadRogue.Primitives;

public class BroadcastMessageArgs(Point source, string message, ushort intensity) : EventArgs
{
    public ushort Intensity { get; } = intensity;
    public string Message { get; } = message;
    public Point Source { get; } = source;
}
