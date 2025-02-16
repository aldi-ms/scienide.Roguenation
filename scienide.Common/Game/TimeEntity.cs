namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;
using System;

public struct TimeEntity(int energy, int speed) : ITimeEntity
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public int Energy { get; set; } = energy;
    public int Speed { get; set; } = speed;
    public int EffectsSumCost { get; set; }
    public IActor? Actor { get; set; } = null;

    public readonly IActionCommand TakeTurn()
    {
        // Handle input to return an action
        return Actor?.TakeTurn() ?? throw new ArgumentNullException(nameof(Actor));
    }
}