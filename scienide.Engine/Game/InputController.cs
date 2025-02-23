namespace scienide.Engine.Game;

using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using scienide.Engine.Game.Actions;
using scienide.Engine.Game.Actors;

public class InputController(Hero actor)
{
    private readonly Hero _actor = actor;

    public bool HandleKeyboard(IScreenObject screenObject, Keyboard keyboard)
    {
        var handled = false;
        var dir = Direction.None;

        if (keyboard.IsKeyPressed(Keys.R)) // Rest
        {
            _actor.Action = new RestAction(_actor);
        }

        /// TODO: controls need refactoring
        #region Cardinal movement
        if (keyboard.IsKeyPressed(Keys.Up) || keyboard.IsKeyPressed(Keys.K))
        {
            dir = Direction.Up;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Down) || keyboard.IsKeyPressed(Keys.J))
        {
            dir = Direction.Down;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Right) || keyboard.IsKeyPressed(Keys.L))
        {
            dir = Direction.Right;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Left) || keyboard.IsKeyPressed(Keys.H))
        {
            dir = Direction.Left;
            handled = true;
        }
        #endregion
        else
        #region Diagonal movement
        if (keyboard.IsKeyPressed(Keys.Y))
        {
            dir = Direction.UpLeft;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.U))
        {
            dir = Direction.UpRight;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.B))
        {
            dir = Direction.DownLeft;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.N))
        {
            dir = Direction.DownRight;
            handled = true;
        }
        #endregion

        if (handled)
        {
            _actor.Action = new WalkAction(_actor, dir);
        }

        return handled;
    }
}
