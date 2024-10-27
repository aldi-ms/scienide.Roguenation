namespace scienide.Engine.Core.Interfaces;

using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using scienide.Engine.Core.Messaging;

public interface IActor : IGameComposite, IMessageSubscriber
{
    Ulid TypeId { get; }
    string Name { get; }
    IGameMap GameMap { get; }
    ITimeEntity? TimeEntity { get; set; }
    IActionCommand? Action { get; set; }
    MessageBroker? MessageBroker { get; set; }

    IActionCommand TakeTurn();
    void Listener(GameMessageArgs args);
}
