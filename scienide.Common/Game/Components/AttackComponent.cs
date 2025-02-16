namespace scienide.Common.Game.Components;

using scienide.Common.Game;

public class AttackComponent : GameComponent
{
    private const int _atk = 1;

    public int Attack()
    {
        return _atk;
    }
}
