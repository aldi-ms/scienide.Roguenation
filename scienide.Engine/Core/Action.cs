using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Core;

public abstract class Action : IAction
{
    public string Name => string.Empty;
    public string Description => string.Empty;

    public abstract void Execute();
    public abstract void Undo();
}
