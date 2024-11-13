namespace scienide.Common.Game.Interfaces;

public interface ITimeEntity
{
    Ulid Id { get; }
    int Energy { get; set; }
    int Speed { get; set; }
    int EffectsSumCost { get; set; }
    IActor? Actor { get; set; }

    IActionCommand TakeTurn();
}
