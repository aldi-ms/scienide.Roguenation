namespace scienide.Engine.Core.Interfaces;

public interface IAction
{
    string Name { get; }
    string Description { get; }
    void Execute();
    void Undo();
}
