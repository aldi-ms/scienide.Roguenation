namespace scienide.Engine.Components;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;

internal class StatsComponent : GameComponent, IDisposable
{
    private bool _disposed = false;
    internal event ActorEventHandler? OnDeath;

    public int MaxHealth { get; set; } = 10;
    public int CurrentHealth { get; set; } = 10;
    public bool IsAlive => CurrentHealth > 0;

    internal void TakeDamage(int dmg)
    {
        if (!IsAlive) return;

        CurrentHealth -= dmg;

        if (CurrentHealth <= 0)
        {
            if (Parent?.Parent is not IActor actor)
            {
                throw new ArgumentNullException(nameof(Parent), $"{nameof(StatsComponent)}.{nameof(Parent)} does not have a parent IActor!");
            }

            MessageBroker.Instance.Broadcast(new ActorDeathMessage(actor));

            OnDeath?.Invoke(this, new ActorArgs(actor));
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        OnDeath = null;
        _disposed = true;
    }
}
