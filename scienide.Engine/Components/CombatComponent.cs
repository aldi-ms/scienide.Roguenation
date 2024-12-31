namespace scienide.Engine.Components;

using scienide.Common.Game;

internal class CombatComponent : GameComposite
{
    public CombatComponent()
    {
        AddComponent(new StatsComponent());
        AddComponent(new AttackComponent());
        AddComponent(new DefenseComponent());
    }
}
