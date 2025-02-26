namespace scienide.Common.Game;

public ref struct ActionResult
{
    public bool Succeeded { get; private set; }
    public bool Finished { get; private set; }

    public int Cost { get; private set; } = 0;

    public ActionCommandBase? AlternativeAction { get; private set; } = null;
    public ActionCommandBase? ContinueWith { get; private set; } = null;

    private ActionResult(bool success, bool done)
    {
        Succeeded = success;
        Finished = done;
    }

    public static ActionResult Success(int cost) => new(true, true) { Cost = cost };
    public static ActionResult Fail() => new(false, true);
    public static ActionResult NotDone(ActionCommandBase continueWith) => new(false, false) { ContinueWith = continueWith };
    public static ActionResult Alternative(ActionCommandBase alternative) => new(true, false) { AlternativeAction = alternative };
}
