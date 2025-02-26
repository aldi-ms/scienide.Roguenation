namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;

public delegate void ActorEventHandler(object? sender, ActorArgs args);

public class ActorArgs(IActor actor) : EventArgs
{
    public IActor Actor { get; set; } = actor;
}