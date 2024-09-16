namespace scienide.Engine.Core.Interfaces;

public interface ITimedEntity
{
    Ulid Id { get; }
    int Energy { get; set; }
    int Speed { get; set; }
    int Cost { get; set; }

    IActionCommand TakeTurn();
}
