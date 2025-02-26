namespace scienide.Common.Game.Components;

using scienide.Common.Game;

public class AttackComponent(int atk) : GameComponent
{
    public int Atk { get; set; } = atk;

    public int Attack() => Atk;
}
