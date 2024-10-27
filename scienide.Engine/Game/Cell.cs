namespace scienide.Engine.Game;

using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Infrastructure;

public class Cell(Point pos) : GameComposite(pos)
{
    private Terrain _terrain;

    public IActor? Actor
    {
        get
        {
            if (GetComponent(GObjType.ActorPlayerControl | GObjType.ActorNonPlayerControl, out IActor? actorComponent))
            {
                return actorComponent;
            }

            return null;
        }
        set
        {
            if (value != null)
            {
                if (!GetComponent(GObjType.ActorPlayerControl | GObjType.ActorNonPlayerControl, out IActor? actorComponent))
                {
                    AddChild(value);
                }
            }
        }
    }

    public new Glyph Glyph
    {
        get
        {
            if (Children == null || Children.Count == 0)
            {
                return base.Glyph;
            }

            var highestOrderElement = Children.OrderByDescending(x => x.Layer).First();
            return highestOrderElement.Glyph;
        }
        set
        {
            base.Glyph = value;
        }
    }

    public Terrain Terrain
    {
        get { return _terrain; }
        set
        {
            _terrain = value;

            var foundChild = Children.SingleOrDefault(x => x.Layer == CollisionLayer.Terrain);
            if (foundChild != null)
            {
                RemoveChild(foundChild);
            }

            AddChild(_terrain);
        }
    }

    public bool IsValidForEntry(GObjType ofType)
    {
        /// TODO
        return Glyph.Char == '.' || Glyph.Char == ',';
    }
}
