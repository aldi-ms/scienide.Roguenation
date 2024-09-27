using scienide.Engine.Game.Actors;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.UnitTests;

public class CircularListTests
{
    [Fact]
    public void ExecuteNode_ShouldRunForNode()
    {
        var circularList = new CircularList();
        var hero = HeroBuilder.CreateBuilder((1, 1))
            .AddGlyph('@')
            .AddTimedEntity(-200, 100, 75)
            .Build();
        var hero2 = HeroBuilder.CreateBuilder((1, 1))
            .AddGlyph('!')
            .AddTimedEntity(-200, 101, 75)
            .Build();

        circularList.Add(hero.TimedEntity ?? throw new ArgumentNullException(nameof(hero.TimedEntity)));
        circularList.Add(hero2.TimedEntity ?? throw new ArgumentNullException(nameof(hero2.TimedEntity)));

        while (true)
        {
            circularList.ProgressSentinel();
            circularList.ProgressTime();
        }
    }
}
