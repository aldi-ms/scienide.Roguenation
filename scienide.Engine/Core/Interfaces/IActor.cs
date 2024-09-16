namespace scienide.Engine.Core.Interfaces;

public interface IActor : IGameComposite
{
    IActionCommand TakeTurn();
    ITimedEntity TimedEntity { get; }
}
