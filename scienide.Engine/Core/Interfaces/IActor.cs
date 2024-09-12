namespace scienide.Engine.Core.Interfaces;

public interface IActor : IGameComposite
{
    IAction TakeTurn();

}
