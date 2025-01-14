using scienide.Common.Game.Interfaces;

namespace scienide.Common.Messaging.Events;

public class ActorDeathMessage(IActor actor) : BaseMessageEvent
{
    public IActor Actor { get; } = actor;
}
