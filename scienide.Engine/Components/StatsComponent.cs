namespace scienide.Engine.Components;

using scienide.Common.Game;

internal class StatsComponent : GameComponent
{
    internal event EventHandler? OnDeath;
    
    public int MaxHealth { get; set; } = 10;
    public int CurrentHealth { get; set; } = 10;
    public bool IsAlive => CurrentHealth > 0;

    internal void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke(this, new EventArgs());
        }
    }
}
