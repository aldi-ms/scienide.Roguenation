namespace scienide.Common.Game;

using scienide.Common.Game.Interfaces;

public abstract class ActionCommandBase(IActor? actor, int cost, string name, string description) : IActionCommand
{
    private static readonly Ulid _id = Ulid.NewUlid();

    private readonly string _name = name;
    private readonly string _description = description;

    public virtual Ulid Id => _id;

    public int Cost { get; set; } = cost;
    public string Name => _name;
    public string Description => _description;

    public IActor? Actor { get; set; } = actor;

    /// <summary>
    /// Execute the action.
    /// </summary>
    /// <returns>Returns the energy cost.</returns>
    public abstract ActionResult Execute();

    public abstract void Undo();

    public abstract string GetActionLog();
}
