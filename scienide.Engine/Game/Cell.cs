using SadRogue.Primitives;
using scienide.Engine.Core;

namespace scienide.Engine.Game;

public class Cell(Point pos) : GameComposite(pos)
{
    private Terrain _terrain;

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

    public bool IsValidForEntry(GameObjType ofType)
    {
        return true;
    }
}
