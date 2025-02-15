namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Engine.Game.Actions;

public class Hero : Actor
{
    #region AutoWalk, for debug & test
    private const bool AutoWalk = false;
    private Direction _dir = Direction.Right;
    #endregion

    public Hero(Point pos, string name) : base(pos, name)
    {
        TypeId = Global.HeroId;
        ObjectType = GObjType.Player;
    }

    public Hero(Point pos) : this(pos, string.Empty)
    {
    }

    public override IActionCommand TakeTurn()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

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

    public override IActor Clone(bool deepClone)
    {
        var hero = new Hero(Position, Name);
        if (deepClone)
        {
            hero.Glyph = Glyph.Clone(deepClone);
            hero.FoVRange = FoVRange;
            // For hero.GameMap to be cloned we need to actually spawn the actor? don't do that for now
            if (TimeEntity != null)
                hero.TimeEntity = new TimeEntity(TimeEntity.Energy, TimeEntity.Speed);
        }
        else
        {
            hero.Glyph = Glyph.Clone(deepClone);
        }

        return hero;
    }
}
