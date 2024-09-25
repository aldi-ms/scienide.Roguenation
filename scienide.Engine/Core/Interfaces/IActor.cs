namespace scienide.Engine.Core.Interfaces;

public interface IActor : IGameComposite
{
    Ulid Id { get; }
    string Name { get; }
    IActionCommand TakeTurn();
    ITimedEntity TimedEntity { get; }
}
