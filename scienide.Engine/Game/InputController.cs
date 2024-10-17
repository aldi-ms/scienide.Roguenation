namespace scienide.Engine.Game;

using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using scienide.Engine.Game.Actions;
using scienide.Engine.Game.Actors;
using System.Diagnostics;

public class InputController(Actor actor)
{
    private readonly Actor _actor = actor;
    private Stopwatch _perfSw = new Stopwatch();
    private int _counter = 0;

    public bool HandleKeyboard(IScreenObject screenObject, Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.Up))
        {
            _actor.Action = new WalkAction(_actor, Direction.Up);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Down))
        {
            _actor.Action = new WalkAction(_actor, Direction.Down);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Right))
        {
            _actor.Action = new WalkAction(_actor, Direction.Right);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Left))
        {
            _actor.Action = new WalkAction(_actor, Direction.Left);
            handled = true;
        }

        if (handled)
        {
            if (_counter == 0)
            {
                _perfSw.Restart();
            }

            _counter++;

            if (_counter == 20)
            {
                _perfSw.Stop();
                Trace.WriteLine($"{nameof(InputController)} [PerfMon]: {_counter} turns elapsed. Ticks: [{_perfSw.ElapsedTicks}]; MilliSeconds: [{_perfSw.ElapsedMilliseconds}]");
                _counter = 0;
            }
        }

        return handled;
    }
}
