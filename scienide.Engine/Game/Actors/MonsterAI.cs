namespace scienide.Engine.Game.Actors;

using scienide.Common.Game.Interfaces;

public abstract class MonsterAI(IActor actor)
{
    public IActor Actor { get; set; } = actor;

    public virtual IActionCommand Act()
    {
        throw new NotImplementedException();
    }
}
