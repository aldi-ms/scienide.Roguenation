namespace scienide.Engine.Core.Interfaces;

public interface ITimedEntity
{
    int Energy { get; set; }
    int Speed { get; set; }
    int Cost { get; set; }

    IActionCommand TakeTurn();
}
