namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

public class Hero : Actor
{
    #region AutoWalk, for debug & test
    private const bool AutoWalk = false;
    private Direction _dir = Direction.Right;
    #endregion

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
            return NoneAction.Instance;
        }

        #region AutoWalk, for debug & test
        if (AutoWalk)
        {
#pragma warning disable CS0162 // Unreachable code detected
            var nextPos = Position + _dir;
            if (!GameMap.IsInValidMapBounds(nextPos.X, nextPos.Y))
            {
                _dir = Direction.GetCardinalDirection(_dir.DeltaX * -1, 0);
            }
#pragma warning restore CS0162 // Unreachable code detected

            return new WalkAction(this, _dir);
        }
        #endregion

        return Action;
    }
}
