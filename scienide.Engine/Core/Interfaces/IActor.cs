namespace scienide.Engine.Core.Interfaces;

public interface IActor : IGameComposite, ITimedEntity
{
    Ulid Id { get; }
    string Name { get; }
}
