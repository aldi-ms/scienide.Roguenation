namespace scienide.Common.Messaging;

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
