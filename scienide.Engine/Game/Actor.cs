using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public abstract class Actor : GameComposite, IActor
{
    private readonly Ulid _id;
    private readonly string _name;
    public string Name => _name;
    public Ulid Id => _id;

    public int Energy { get; set; }
    public int Speed { get; set; }
    public int Cost { get; set; }

    public Actor(string name, Point pos, Glyph glyph) 
        : base(pos)
    {
        _id = Ulid.NewUlid();
        _name = name;
        Glyph = glyph;
    }

    public abstract IActionCommand TakeTurn();
}
