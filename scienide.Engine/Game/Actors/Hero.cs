namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

public class Hero : Actor
{
    // for debug & test purposes
    private const bool AutoWalk = false;

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
    private Direction _dir = Direction.Right;
    public override IActionCommand TakeTurn()
    {
        if (Action == null)
        {
            return NoneAction.Instance;
        }

        if (AutoWalk)
        {
#pragma warning disable CS0162 // Unreachable code detected
            if (!GameMap.IsValidPosition(Position + _dir))
            {
                _dir = Direction.GetCardinalDirection(_dir.DeltaX * -1, 0);
            }
#pragma warning restore CS0162 // Unreachable code detected

            return new WalkAction(this, _dir);
        }

        return Action;
    }
}
