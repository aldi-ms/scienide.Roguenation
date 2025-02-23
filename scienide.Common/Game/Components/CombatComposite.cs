namespace scienide.Common.Game.Components;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

public class CombatComposite : GameComposite, IDisposable
{
    private bool _disposed = false;

    private readonly StatsComponent _stats;
    private readonly AttackComponent _atk;
    private readonly DefenseComponent _def;

    public CombatComposite(ActorCombatStats actorStats)
    {
        _stats = new StatsComponent(actorStats.MaxHealth);
        _atk = new AttackComponent(actorStats.Attack);
        _def = new DefenseComponent(actorStats.Defense);

        AddComponent(_stats);
        AddComponent(_atk);
        AddComponent(_def);
    }

    public void MeleeAttack(IActor target)
    {
        var outgoingAttackDamage = _atk.Attack();

        if (!target.TryGetComponent<DefenseComponent>(out var targetDefense))
        {
            throw new ArgumentException($"{target.Name} does not have a {nameof(DefenseComponent)}!");
        }

        var ingoingDamage = outgoingAttackDamage - targetDefense.Defense;

        _stats.TakeDamage(ingoingDamage);
    }

    public void Dispose()
    {
        if (_disposed) return;

        _stats.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

public struct ActorCombatStats
{
    public int MaxHealth;
    public int Defense;
    public int Attack;
}