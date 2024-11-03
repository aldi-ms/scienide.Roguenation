namespace scienide.Common.Messaging;

using SadRogue.Primitives;
using scienide.Common.Messaging.Events;

public class MessageBroker
{
    public delegate bool MessageReceiveHeuristic(Point p1, Point p2, int intensity);

    private static readonly Type _messageSubType = typeof(IMessageSubscriber);

    private static readonly Lazy<MessageBroker> _instance = new(() => new MessageBroker(), true);

    private MessageBroker()
    {

    }

    public static MessageBroker Instance => _instance.Value;

    private readonly Dictionary<Type, List<IActorListener>> _eventListeners = [];

    public MessageReceiveHeuristic ShouldReceiveMessage { get; set; } = DefaultShouldReceiveMsg;

    public void Broadcast<T>(T eventArgs) where T : EventArgs
    {
        var eventType = typeof(T);
        if (_eventListeners.TryGetValue(eventType, out var subscribers))
        {
            foreach (var subscriber in subscribers)
            {
                if (eventArgs is not GameMessageEventArgs messageArgs
                    || ShouldReceiveMessage.Invoke(messageArgs.Source, subscriber.Subscriber.Position, messageArgs.Intensity))
                {
                    subscriber.Invoke(eventArgs);
                }
            }
        }
    }

    public void Subscribe<T>(Action<T> listener, IMessageSubscriber? sub = null) where T : EventArgs
    {
        Type eventType = typeof(T);
        if (eventType == _messageSubType && sub == null)
        {
            throw new ArgumentNullException(nameof(sub));
        }

        if (!_eventListeners.TryGetValue(eventType, out var value))
        {
            value = [];
            _eventListeners[eventType] = value;
        }

        value.Add(new ActorListener<T>(listener, sub));
    }

    public void Unsubscribe<T>(Action<T> listener, IMessageSubscriber? sub = null) where T : EventArgs
    {
        Type eventType = typeof(T);

        if (_eventListeners.TryGetValue(eventType, out var listeners))
        {
            // Find the listener to remove
            listeners.RemoveAll(l =>
                l is ActorListener<T> typedListener &&
                (sub == null || typedListener.Subscriber == sub) &&
                typedListener.Listener == listener);

            if (listeners.Count == 0)
            {
                _eventListeners.Remove(eventType);
            }
        }
    }

    internal static bool DefaultShouldReceiveMsg(Point p1, Point p2, int intensity)
    {
        // Manhattan distance - movement only in 4 directions
        var distance = MathF.Abs(p1.X - p2.X) + MathF.Abs(p1.Y - p2.Y);

        // Euclidean distance - straight line
        // var distance2 = MathF.Sqrt(MathF.Pow(p1.X - p2.X, 2) + MathF.Pow(p1.Y - p2.Y, 2));

        return distance <= intensity;
    }

    private interface IActorListener
    {
        IMessageSubscriber Subscriber { get; }
        void Invoke(EventArgs e);
    }

    private class ActorListener<T>(Action<T> listener, IMessageSubscriber sub) : IActorListener where T : EventArgs
    {
        public IMessageSubscriber Subscriber { get; set; } = sub;
        public Action<T> Listener { get; set; } = listener;

        public void Invoke(EventArgs e)
        {
            // Cast the event to the specific type and invoke the listener
            if (e is T typedEvent)
            {
                Listener(typedEvent);
            }
        }
    }

}
