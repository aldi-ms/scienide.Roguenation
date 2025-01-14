namespace scienide.Common.Messaging;

using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging.Events;
using System.Collections.Concurrent;

public interface IMessageSubscriber : ILocatable;

public class MessageBroker
{
    private static readonly Type _messageSubType = typeof(IMessageSubscriber);
    private static readonly Lazy<MessageBroker> _instance = new(() => new MessageBroker(), true);

    private readonly ConcurrentDictionary<Type, List<IActorListener>> _eventListeners = [];

    private MessageBroker()
    {
    }

    public static MessageBroker Instance => _instance.Value;

    public void Broadcast<T>(T eventArgs, MessageScope scope = MessageScope.Global) where T : BaseMessageEvent
    {
        var eventType = typeof(T);
        if (_eventListeners.TryGetValue(eventType, out var listeners))
        {
            foreach (var listener in listeners.ToArray())
            {
                if (listener.ShouldReceive(eventArgs))
                {
                    listener.Invoke(eventArgs, scope);
                }
            }
        }
    }

    public void Subscribe<T>(Action<T> handler, IMessageSubscriber? subscriber = null, MessageScope scope = MessageScope.Global) where T : BaseMessageEvent
    {
        Type eventType = typeof(T);
        if (eventType == _messageSubType && subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }

        if (!_eventListeners.TryGetValue(eventType, out var listeners))
        {
            listeners = [];
            _eventListeners[eventType] = listeners;
        }

        listeners.Add(new ActorListener<T>(handler, subscriber!, scope));
    }

    public void Unsubscribe<T>(Action<T> handler, IMessageSubscriber subscriber) where T : BaseMessageEvent
    {
        Type eventType = typeof(T);
        if (_eventListeners.TryGetValue(eventType, out var listeners))
        {
            // Find the listener to remove
            listeners.RemoveAll(l =>
                l is ActorListener<T> typedListener
                && typedListener.Subscriber == subscriber
                && typedListener.Handler == handler);

            if (listeners.Count == 0)
            {
                _eventListeners.TryRemove(eventType, out var _);
            }
        }
    }

    private interface IActorListener
    {
        void Invoke(EventArgs e, MessageScope scope);

        bool ShouldReceive(BaseMessageEvent e);
    }

    private class ActorListener<T>(Action<T> handler, IMessageSubscriber sub, MessageScope scope) : IActorListener where T : BaseMessageEvent
    {
        public IMessageSubscriber Subscriber { get; set; } = sub;
        public Action<T> Handler { get; set; } = handler;
        public MessageScope Scope { get; } = scope;

        public void Invoke(EventArgs e, MessageScope scope)
        {
            // Cast the event to the specific type and invoke the listener
            if (e is T typedEvent && scope == Scope)
            {
                Handler(typedEvent);
            }
        }

        public bool ShouldReceive(BaseMessageEvent e)
        {
            if (e is GameMessage gameMessage)
            {
                var distance = MathF.Sqrt(
                    MathF.Pow(gameMessage.Source.X - Subscriber.Position.X, 2) +
                    MathF.Pow(gameMessage.Source.Y - Subscriber.Position.Y, 2)
                );
                return distance <= gameMessage.Intensity;
            }

            return true; // Default behavior: accept all other messages
        }
    }
}
