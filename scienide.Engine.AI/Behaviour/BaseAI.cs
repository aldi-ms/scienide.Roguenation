namespace scienide.Engine.AI.Behaviour;

using scienide.Common.Game.Interfaces;

public abstract class BaseAI(IActor actor)
{
    public IActor Actor { get; set; } = actor;

    public abstract IActionCommand Act();
}
