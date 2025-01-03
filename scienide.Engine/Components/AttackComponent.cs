namespace scienide.Engine.Components;

using scienide.Common.Game;

internal class AttackComponent : GameComponent
{
    private const int _atk = 1;

    public int Attack()
    {
        return _atk;
    }
}
