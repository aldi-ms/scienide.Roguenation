namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

public class Hero : Actor
{
    public Hero(Point pos, string name) : base(pos)
    {
        TypeId = Global.HeroId;
        //Name = name;
    }

    public Hero(Point pos) : this(pos, string.Empty)
    {
    }

    public override IActionCommand TakeTurn()
    {
        if (Action == null)
        {
            return new NoneAction();
        }

        return Action;
    }
}
