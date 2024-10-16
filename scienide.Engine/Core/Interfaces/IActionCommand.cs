namespace scienide.Engine.Core.Interfaces;

public interface IActionCommand
{
    IActor? Actor { get; set; }
    Ulid Id { get; }
    string Name { get; }
    string Description { get; }
    int Cost { get; set; }
    int Execute();
    void Undo(); 
    string GetActionLog();
}
