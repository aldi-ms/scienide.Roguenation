using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Core;

public abstract class ActionCommand(IActor? actor, int cost, string name, string description) : IActionCommand
{
    private readonly string _name = name;
    private readonly string _description = description;

    public string Name => _name;
    public string Description => _description;
    public int Cost { get; set; } = cost;

    public IActor? Actor { get; set; } = actor;

    /// <summary>
    /// Execute the action.
    /// </summary>
    /// <returns>Returns the energy cost.</returns>
    public abstract int Execute();
    public abstract void Undo();

    public abstract string GetActionLog();
}
