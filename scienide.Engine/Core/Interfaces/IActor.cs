namespace scienide.Engine.Core.Interfaces;

public interface IActor : IGameComposite
{
    Ulid Id { get; }
    string Name { get; }
    ITimedEntity? TimedEntity { get; set; }
}
