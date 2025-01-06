namespace scienide.Engine.Components;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

internal class CombatComposite : GameComposite, IDisposable
{
    private bool _disposed = false;
    internal event ActorEventHandler? OnDeath = delegate { };

    private readonly StatsComponent _stats;
    private readonly AttackComponent _atk;
    private readonly DefenseComponent _def;

    public CombatComposite()
    {
        _stats = new StatsComponent();
        _stats.OnDeath += HandleDeath;
        _atk = new AttackComponent();
        _def = new DefenseComponent();

        AddComponent(_stats);
        AddComponent(_atk);
        AddComponent(_def);
    }

    private void HandleDeath(object? sender, ActorArgs e)
    {
        OnDeath?.Invoke(sender, e);
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

        OnDeath = null;
        _stats.Dispose();
        _disposed = true;
    }
}
