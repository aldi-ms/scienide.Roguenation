namespace scienide.Engine.Components;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

internal class CombatComposite : GameComposite
{
    internal event EventHandler? OnDeath;
    
    private readonly StatsComponent _stats;
    private readonly AttackComponent _atk;
    private readonly DefenseComponent _def;

    public CombatComposite()
    {
        _stats = new StatsComponent();
        _stats.OnDeath += OnDeath;
        _atk = new AttackComponent();
        _def = new DefenseComponent();

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
}
