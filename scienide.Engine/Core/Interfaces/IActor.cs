namespace scienide.Engine.Core.Interfaces;

public interface IActor : IGameComposite
{
    Ulid Id { get; }
    string Name { get; }
    ITimeEntity? TimeEntity { get; set; }
    IActionCommand? Action { get; set; }
    IActionCommand TakeTurn();
}
