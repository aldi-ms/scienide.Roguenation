namespace scienide.Engine.UnitTests;

using SadRogue.Primitives;
using scienide.Engine.Game.Actors.Builder;

public class BuilderTests
{
    [Fact]
    public void X()
    {
        var builder = new HeroBuilder(Point.Zero);
    }
}
