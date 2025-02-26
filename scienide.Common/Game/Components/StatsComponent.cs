namespace scienide.Common.Game.Components;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging;

public class StatsComponent(int maxHealth) : GameComponent, IDisposable
{
    private bool _disposed = false;

    public int MaxHealth { get; set; } = maxHealth;
    public int CurrentHealth { get; set; } = maxHealth;
    public bool IsAlive => CurrentHealth > 0;

    public void TakeDamage(int dmg)
    {
        if (!IsAlive || dmg <= 0) return;

        CurrentHealth -= dmg;

        if (CurrentHealth <= 0)
        {
            if (Parent?.Parent is not IActor actor)
            {
                throw new ArgumentNullException(nameof(Parent), $"{nameof(StatsComponent)}.{nameof(Parent)} does not have a parent IActor!");
            }

            MessageBroker.Instance.Broadcast(new ActorDeathMessage(actor), false);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        GC.SuppressFinalize(this);
    }
}