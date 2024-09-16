namespace scienide.Engine.Core.Interfaces;

public interface IActor : IGameComposite
{
    string Name { get; }
    IActionCommand TakeTurn();
    ITimedEntity TimedEntity { get; }
}
