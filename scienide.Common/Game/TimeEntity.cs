namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;

public abstract class TimeEntity(int energy, int speed) : ITimeEntity
{
    public int Energy { get; set; } = energy;
    public int Speed { get; set; } = speed;
    public int EffectsSumCost { get; set; }
    public IActor? Actor { get; set; } = null;

    public abstract IActionCommand TakeTurn();
}

public class ActorTimeEntity(int energy, int speed) : TimeEntity(energy, speed)
{
    public override IActionCommand TakeTurn()
    {
        // Handle input to return an action
        return Actor?.TakeTurn() ?? throw new ArgumentNullException(nameof(Actor));
    }
}

/// <summary>
/// Time entity that throws an <see cref="InvalidOperationException"/> when <see cref="TakeTurn"/> is called.
/// </summary>
public class CrashTurnTimeEntity : TimeEntity
{
    // Set to non-zero energy to be available to TakeTurn
    public CrashTurnTimeEntity() : base(0, 1)
    {
    }

    public override IActionCommand TakeTurn()
    {
        throw new InvalidOperationException($"{nameof(CrashTurnTimeEntity)}.{nameof(TakeTurn)}() should never be called!");
    }
}

public class FuncTimeEntity(int energy, int speed, Func<IActionCommand> func) : TimeEntity(energy, speed)
{
    public Func<IActionCommand> Func { get; set; } = func;

    public override IActionCommand TakeTurn()
    {
        return Func();
    }
}