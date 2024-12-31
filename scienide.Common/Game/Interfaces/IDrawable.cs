namespace scienide.Common.Game.Interfaces;
public interface IDrawable : IGameComponent
{
    public Glyph Glyph { get; }
    public Layer Layer { get; }
}
