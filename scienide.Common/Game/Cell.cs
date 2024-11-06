namespace scienide.Common.Game;

using SadRogue.Primitives;
using scienide.Common.Game.Interfaces;
using scienide.Common.Infrastructure;

public class Cell : GameComposite
{
    private Terrain _terrain;
    private BitProperties _properties;

    public Cell(Point pos) : base(pos)
    {
        _properties = new BitProperties();
    }

    public IActor? Actor
    {
        get
        {
            if (TryGetComponent(GObjType.ActorPlayerControl | GObjType.ActorNonPlayerControl, out IActor? actorComponent))
            {
                return actorComponent;
            }

            return null;
        }
        set
        {
            if (value != null)
            {
                if (!TryGetComponent(GObjType.ActorPlayerControl | GObjType.ActorNonPlayerControl, out IActor? actorComponent))
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

            // TODO:
            _properties.SetProperty(NamedBits.BlocksLight, _terrain.Glyph.Char == '#');
        }
    }

    public BitProperties Properties => _properties;

    public bool IsValidForEntry(GObjType ofType)
    {
        /// TODO
        return Glyph.Char == '.' || Glyph.Char == ',' || Glyph.Char == ' ';
    }
}
