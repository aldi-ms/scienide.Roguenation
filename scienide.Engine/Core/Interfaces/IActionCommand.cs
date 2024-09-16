namespace scienide.Engine.Core.Interfaces;

public interface IActionCommand
{
    string Name { get; }
    string Description { get; }
    int Execute();
    void Undo();
}
