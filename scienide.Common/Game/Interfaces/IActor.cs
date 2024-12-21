namespace scienide.Common.Game.Interfaces;

using scienide.Common.Messaging;

public interface IActor : IGameComposite, IMessageSubscriber, IGenericCloneable<IActor>
{
    int FoVRange { get; set; }
    string Name { get; }
    Ulid TypeId { get; }
    IGameMap GameMap { get; }
    ITimeEntity? TimeEntity { get; set; }
    IActionCommand? Action { get; set; }
    Cell CurrentCell { get; }

    void SubscribeForMessages();
    void UnsubscribeFromMessages();
    IActionCommand TakeTurn();
}
