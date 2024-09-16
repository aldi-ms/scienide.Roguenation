using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Core;

public abstract class ActionCommand(string name, string description) : IActionCommand
{
    private readonly string _name = name;
    private readonly string _description = description;

    public string Name => _name;
    public string Description => _description;

    public abstract int Execute();
    public abstract void Undo();
}
