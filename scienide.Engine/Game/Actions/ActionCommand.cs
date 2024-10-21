using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game.Actions;

public abstract class ActionCommand(IActor? actor, int cost, string name, string description) : IActionCommand
{
    private readonly string _name = name;
    private readonly string _description = description;
    private readonly static Ulid _id = Ulid.NewUlid();

    public Ulid Id => _id;
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


public class NoneAction : IActionCommand
{
    public Ulid Id => Global.NoneActionId;

    public IActor? Actor { get; set; }

    public string Name => string.Empty;

    public string Description => string.Empty;

    public int Cost { get => 0; set => throw new NotImplementedException(); }

    public int Execute()
    {
        return Cost;
    }

    public string GetActionLog()
    {
        throw new NotImplementedException();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}