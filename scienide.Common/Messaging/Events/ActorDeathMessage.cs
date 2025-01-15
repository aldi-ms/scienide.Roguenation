namespace scienide.Common.Messaging.Events;

using scienide.Common.Game.Interfaces;

public class ActorDeathMessage(IActor actor) : BaseMessageEvent
{
    public IActor Actor { get; } = actor;
}
