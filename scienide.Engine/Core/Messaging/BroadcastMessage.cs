namespace scienide.Engine.Core.Messaging;

using SadRogue.Primitives;

public class BroadcastMessage(Point source, string message, ushort intensity)
{
    public ushort Intensity { get; } = intensity;
    public string Message { get; } = message;
    public Point SourcePos { get; } = source;
}
