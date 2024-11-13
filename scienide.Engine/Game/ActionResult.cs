namespace scienide.Engine.Game;

using scienide.Common.Game;

public ref struct ActionResult(bool success, bool done)
{
    //public static readonly ActionResult Success = new(true, true);
    //public static readonly ActionResult Failure = new(false, true);
    //public static readonly ActionResult NotDone = new(true, false);

    public bool Succeeded { get; private set; } = success;
    public bool Finished { get; private set; } = done;

    public ActionCommandBase? Alternative { get; private set; } = null;
    public ActionCommandBase? ContinueWith { get; private set; } = null;

    public ActionResult(ActionCommandBase alternative)
        : this(false, true)
    {
        Alternative = alternative;
    }
}
