namespace scienide.Common.Messaging.Events;

public abstract class BaseMessageEvent : EventArgs
{
    public MessageScope Scope { get; set; } = MessageScope.Global;
}

public enum MessageScope
{
    Global,
    Local,
    UI
}
