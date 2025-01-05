namespace scienide.Common.Messaging.Events;

public abstract class MessageEvent : EventArgs
{
    public MessageScope Scope { get; set; } = MessageScope.Global;
}

public enum MessageScope
{
    Global,
    Local,
    UI
}
