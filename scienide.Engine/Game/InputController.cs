namespace scienide.Engine.Game;

using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;
using scienide.Engine.Game.Actors;

public class InputController
{
    private readonly Hero _actor;
    private readonly Dictionary<Keys, ActionCommandBase> _actionBindings = [];

    public InputController(Hero actor)
    {
        _actor = actor;
    }

    public bool HandleKeyboard(IScreenObject screenObject, Keyboard keyboard)
    {
        var handled = false;
        ActionCommandBase command = NoneAction.Instance;

        foreach (var k in keyboard.KeysPressed)
        {
            switch (k.Key)
            {
                case Keys.R:
                    command = new RestAction(_actor);
                    handled = true;
                    break;

                case Keys.Up:
                case Keys.K:
                    command = new WalkAction(_actor, Direction.Up);
                    handled = true;
                    break;

                case Keys.Down:
                case Keys.J:
                    command = new WalkAction(_actor, Direction.Down);
                    handled = true;
                    break;

                case Keys.Left:
                case Keys.H:
                    command = new WalkAction(_actor, Direction.Left);
                    handled = true;
                    break;

                case Keys.Right:
                case Keys.L:
                    command = new WalkAction(_actor, Direction.Right);
                    handled = true;
                    break;

                case Keys.Y:
                    command = new WalkAction(_actor, Direction.UpLeft);
                    handled = true;
                    break;

                case Keys.U:
                    command = new WalkAction(_actor, Direction.UpRight);
                    handled = true;
                    break;

                case Keys.B:
                    command = new WalkAction(_actor, Direction.DownLeft);
                    handled = true;
                    break;

                case Keys.N:
                    command = new WalkAction(_actor, Direction.DownRight);
                    handled = true;
                    break;

                default:
                    break;
            }
        }

        if (handled)
        {
            _actor.Action = command;
        }

        return handled;
    }
}
