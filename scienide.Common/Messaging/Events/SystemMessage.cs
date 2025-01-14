namespace scienide.Common.Messaging.Events;

public class SystemMessage(string message, string? source = null) : BaseMessageEvent
{
    public string? Source { get; } = source;
    public string Message { get; } = message;
}
