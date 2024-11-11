namespace scienide.Engine.Game.Actions;

using scienide.Common;
using scienide.Common.Game;
using System;

public class NoneAction() : ActionCommand(null, 0, string.Empty, string.Empty)
{
    public override Ulid Id => Global.NoneActionId;

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