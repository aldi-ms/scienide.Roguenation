namespace scienide.Common.Messaging.Events;

public class SystemMessageArgs(string message, string? source = null) : MessageEvent
{
    public string? Source { get; } = source;
    public string Message { get; } = message;
}
