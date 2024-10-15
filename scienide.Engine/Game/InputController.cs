namespace scienide.Engine.Game;

using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using scienide.Engine.Game.Actions;
using scienide.Engine.Game.Actors;
using System.Diagnostics;

public class InputController(Hero hero)
{
    private readonly Hero _hero = hero;
    private Stopwatch _perfSw = new Stopwatch();
    private int _counter = 0;

    public bool HandleKeyboard(IScreenObject screenObject, Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.Up))
        {
            _hero.Action = new WalkAction(_hero, Direction.Up);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Down))
        {
            _hero.Action = new WalkAction(_hero, Direction.Down);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Right))
        {
            _hero.Action = new WalkAction(_hero, Direction.Right);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Left))
        {
            _hero.Action = new WalkAction(_hero, Direction.Left);
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
