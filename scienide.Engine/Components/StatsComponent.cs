namespace scienide.Engine.Components;

using scienide.Common.Game;

internal class StatsComponent : GameComponent
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public bool IsAlive => CurrentHealth > 0;
}
