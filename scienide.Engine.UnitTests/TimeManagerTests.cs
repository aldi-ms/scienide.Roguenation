namespace scienide.Engine.UnitTests;

using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Engine.Game.Actions;
using scienide.Engine.Game.Actors.Builder;

public class TimeManagerTests
{
    [Fact]
    public void TimeManager_WithTwoEntities_FasterEntityShouldTakeTurnFirst()
    {
        var hero = new HeroBuilder((1, 1))
            .SetGlyph('@')
            .Build();
        var hero2 = new HeroBuilder((1, 1))
            .SetGlyph('!')
            .Build();
        bool heroTookTurn = false;
        bool hero2TookTurn = false;
        //hero.TimeEntity = new FuncTimeEntity(-2, 1, () =>
        //{
        //    heroTookTurn = true;
        //    return new RestAction(hero);
        //});
        //hero2.TimeEntity = new FuncTimeEntity(-2, 2, () =>
        //{
        //    hero2TookTurn = true;
        //    return new RestAction(hero2);
        //});

        var timeManager = new TimeManager();
        timeManager.Add(hero.TimeEntity ?? throw new ArgumentNullException(nameof(hero.TimeEntity)));
        timeManager.Add(hero2.TimeEntity ?? throw new ArgumentNullException(nameof(hero2.TimeEntity)));

        for (int i = 0; i < 2; i++)
        {
            timeManager.ProgressSentinel();
            timeManager.ProgressTime();
        }

        Assert.True(hero2TookTurn && !heroTookTurn);
    }

    [Fact]
    public async Task TimeManager_WithOneEntity_ShouldTakeTurn()
    {
        var hero = new HeroBuilder((1, 1))
            .SetGlyph('@')
            //.SetTimeEntity(new CrashTurnTimeEntity())
            .Build();

        var timeManager = new TimeManager();
        timeManager.Add(hero.TimeEntity ?? throw new ArgumentNullException(nameof(hero.TimeEntity)));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            while (true)
            {
                timeManager.ProgressSentinel();
                timeManager.ProgressTime();
            }
        });
    }

    [Fact]
    public void ExecuteTimeManager_Empty_ShouldRun()
    {
        var timeManager = new TimeManager();
        var exception = Record.Exception(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                timeManager.ProgressSentinel();
                timeManager.ProgressTime();
            }
        });

        Assert.Null(exception);
    }
}