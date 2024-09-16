using ActionCommand = scienide.Engine.Core.ActionCommand;

namespace scienide.Engine.Game;

public ref struct ActionResult(bool success, bool done)
{
    //public readonly static ActionResult Success = new(true, true);
    //public readonly static ActionResult Failure = new(false, true);
    //public readonly static ActionResult NotDone = new(true, false);

    public ActionCommand? Alternative { get; private set; } = null;
    public ActionCommand? ContinueWith { get; private set; } = null;

    public bool Succeeded { get; private set; } = success;
    public bool Finished { get; private set; } = done;

    public ActionResult(ActionCommand alternative)
        : this(false, true)
    {
        Alternative = alternative;
    }
}
