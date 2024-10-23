namespace scienide.Engine.Core.Interfaces;

using scienide.Engine.Core.Messaging;

public interface IActor : IGameComposite
{
    Ulid TypeId { get; }
    string Name { get; }
    IGameMap GameMap { get; }
    ITimeEntity? TimeEntity { get; set; }
    IActionCommand? Action { get; set; }
    MessageBroker? MessageBroker { get; set; }

    void MakeNoise(string msg);
    IActionCommand TakeTurn();
}
