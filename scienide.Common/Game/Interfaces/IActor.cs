namespace scienide.Common.Game.Interfaces;

using scienide.Common.Messaging;

public interface IActor : IGameComposite, IDrawable, IMessageSubscriber, IGenericCloneable<IActor>, ILocatable, IDisposable
{
    int FoVRange { get; set; }
    string Name { get; }
    Ulid Id { get; }
    IGameMap GameMap { get; }
    ITimeEntity? TimeEntity { get; set; }
    IActionCommand? Action { get; set; }
    Cell CurrentCell { get; }

    void ClearAction();
    void SubscribeForMessages();
    void UnsubscribeFromMessages();
    IActionCommand TakeTurn();
    Dictionary<string, string> FetchComponentShortData();
}
