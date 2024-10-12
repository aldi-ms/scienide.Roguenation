namespace scienide.Engine.UnitTests;

using scienide.Engine.Game.Actors;
using scienide.Engine.Infrastructure;

public class TimeListTests
{
    [Fact]
    public void ExecuteTimeList_WithTwoEntities_ShouldTakeTurns()
    {
        var circularList = new TimeManager();
        var hero = HeroBuilder.CreateBuilder((1, 1))
            .AddGlyph('@')
            .SetTimeEntity(-200, 100, 75)
            .Build();
        var hero2 = HeroBuilder.CreateBuilder((1, 1))
            .AddGlyph('!')
            .SetTimeEntity(-200, 101, 75)
            .Build();

        circularList.Add(hero.TimeEntity ?? throw new ArgumentNullException(nameof(hero.TimeEntity)));
        circularList.Add(hero2.TimeEntity ?? throw new ArgumentNullException(nameof(hero2.TimeEntity)));

        while (true)
        {
            circularList.ProgressSentinel();
            circularList.ProgressTime();
        }
    }
    [Fact]
    public void ExecuteTimeList_WithOneEntity_ShouldCallTakeTurn()
    {
        var circularList = new TimeManager();
    }
}
