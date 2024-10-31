namespace scienide.Engine.Game;

using ActionCommand = Actions.ActionCommand;

public ref struct ActionResult(bool success, bool done)
{
    //public static readonly ActionResult Success = new(true, true);
    //public static readonly ActionResult Failure = new(false, true);
    //public static readonly ActionResult NotDone = new(true, false);

    public bool Succeeded { get; private set; } = success;
    public bool Finished { get; private set; } = done;

    public ActionCommand? Alternative { get; private set; } = null;
    public ActionCommand? ContinueWith { get; private set; } = null;

    public ActionResult(ActionCommand alternative)
        : this(false, true)
    {
        Alternative = alternative;
    }
}
