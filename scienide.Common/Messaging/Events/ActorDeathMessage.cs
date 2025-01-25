namespace scienide.Common.Messaging.Events;

using scienide.Common.Game.Interfaces;

public class ActorDeathMessage : BaseMessageEvent
{
    public IActor Actor { get; }

    public ActorDeathMessage(IActor actor)
    {
        Actor = actor;
        Consume = true;
    }
}