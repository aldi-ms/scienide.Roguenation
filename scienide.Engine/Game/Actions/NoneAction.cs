namespace scienide.Engine.Game.Actions;

using scienide.Common;
using scienide.Common.Game;
using System;

public class NoneAction : ActionCommandBase
{
    public static readonly NoneAction Instance = new();

    public override Ulid Id => Global.NoneActionId;

    private NoneAction() : base(null, 0, string.Empty, string.Empty)
    {

    }

    public override int Execute()
    {
        return 0;
    }

    public override string GetActionLog()
    {
        throw new NotImplementedException();
    }

    public override void Undo()
    {
        throw new NotImplementedException();
    }
}