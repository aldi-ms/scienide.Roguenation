namespace scienide.Common.Messaging.Events;

public class SystemMessageEventArgs(string message, string? source = null) : EventArgs
{
    public string? Source { get; } = source;
    public string Message { get; } = message;
}
