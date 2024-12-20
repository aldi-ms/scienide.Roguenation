namespace scienide.Engine.Game.Actors;

using scienide.Common.Game.Interfaces;

public abstract class BaseAI(IActor actor)
{
    public IActor Actor { get; set; } = actor;

    public abstract IActionCommand Act();
}
