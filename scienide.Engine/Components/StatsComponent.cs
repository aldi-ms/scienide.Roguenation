namespace scienide.Engine.Components;

using scienide.Common.Game;

public class StatsComponent : GameComposite
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
}
