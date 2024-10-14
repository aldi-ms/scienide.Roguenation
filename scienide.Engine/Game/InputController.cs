namespace scienide.Engine.Game;

using SadConsole.Input;
using SadConsole;
using scienide.Engine.Game.Actors;
using scienide.Engine.Game.Actions;
using SadRogue.Primitives;

public class InputController(Hero hero)
{
    private readonly Hero _hero = hero;

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

        return handled;
    }
}
