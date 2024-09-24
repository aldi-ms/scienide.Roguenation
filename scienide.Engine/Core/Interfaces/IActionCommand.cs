namespace scienide.Engine.Core.Interfaces;

public interface IActionCommand
{
    //IActor Actor { get; set; }
    string Name { get; }
    string Description { get; }
    int Execute();
    void Undo();
}
