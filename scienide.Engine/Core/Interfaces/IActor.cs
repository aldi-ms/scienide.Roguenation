namespace scienide.Engine.Core.Interfaces;

public interface IActor : IGameComposite
{
    Ulid TypeId { get; }
    string Name { get; }
    ITimeEntity? TimeEntity { get; set; }
    IActionCommand? Action { get; set; }
    IGameMap GameMap { get; }
    IActionCommand TakeTurn();
}
