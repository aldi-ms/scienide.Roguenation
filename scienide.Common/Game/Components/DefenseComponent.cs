namespace scienide.Common.Game.Components;

using scienide.Common.Game;

public class DefenseComponent(int def) : GameComponent
{
    public int Defense { get; set; } = def;
}
