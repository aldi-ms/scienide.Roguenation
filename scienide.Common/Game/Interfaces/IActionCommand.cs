namespace scienide.Common.Game.Interfaces;

public interface IActionCommand
{
    IActor? Actor { get; set; }
    Ulid Id { get; }
    string Name { get; }
    string Description { get; }
    int Cost { get; set; }
    ActionResult Execute();
    void Undo();
    string GetActionLog();
}
