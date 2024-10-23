namespace scienide.Engine.Core.Messaging;

using SadRogue.Primitives;
using scienide.Engine.Core.SoundSystem;

public class MessageBroker
{
    public delegate bool MessageReceiveHeuristic(BroadcastMessage message, IMessageListener listener);

    private readonly List<IMessageListener> _listeners = [];

    public MessageReceiveHeuristic MessageReceive { get; set; } = DefaultShouldReceiveMsg;

    public void BroadcastMessage(BroadcastMessage broadcastMsg)
    {
        for (var i = 0; i < _listeners.Count; i++)
        {
            if (MessageReceive(broadcastMsg, _listeners[i]))
            {
                _listeners[i].OnMessageReceived(broadcastMsg.Message);
            }
        }
    }

    public void RegisterListener(IMessageListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnregisterListener(IMessageListener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }

    internal static bool DefaultShouldReceiveMsg(BroadcastMessage message, IMessageListener listener)
    {
        var distance = Point.EuclideanDistanceMagnitude(message.SourcePos, listener.Actor.Position);

        return distance <= message.Intensity;
    }
}
