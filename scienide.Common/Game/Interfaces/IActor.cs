namespace scienide.Common.Game.Interfaces;

using scienide.Common.Messaging;

public interface IActor : IGameComposite, IMessageSubscriber, IGenericCloneable<IActor>
{
    Ulid TypeId { get; }
    string Name { get; }
    IGameMap GameMap { get; }
    ITimeEntity? TimeEntity { get; set; }
    IActionCommand? Action { get; set; }

    IActionCommand TakeTurn();
    void SubscribeForMessages();
    void UnsubscribeFromMessages();
}
