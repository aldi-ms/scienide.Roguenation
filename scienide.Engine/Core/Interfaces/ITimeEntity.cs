namespace scienide.Engine.Core.Interfaces;

public interface ITimeEntity
{
    int Energy { get; set; }
    int Speed { get; set; }
    int Cost { get; set; }
    IActor? Actor { get; set; }

    IActionCommand TakeTurn();
}
