namespace scienide.Common.Game.Interfaces;

using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;

public interface IActor : IGameComposite, IMessageSubscriber
{
    Ulid TypeId { get; }
    string Name { get; }
    IGameMap GameMap { get; }
    ITimeEntity? TimeEntity { get; set; }
    IActionCommand? Action { get; set; }

    IActionCommand TakeTurn();
    void Listener(GameMessageEventArgs args);
}
