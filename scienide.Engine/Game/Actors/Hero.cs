namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

public class Hero : Actor
{
    public Hero(Point pos, string name) : base(pos)
    {
        TypeId = Global.HeroId;
        Name = name;
        FoVRange = 10;
    }

    public Hero(Point pos) : this(pos, string.Empty)
    {
    }

    public int FoVRange { get; set; }

    public override IActionCommand TakeTurn()
    {
        if (Action == null)
        {
            return new NoneAction();
        }

        return Action;
    }
}
