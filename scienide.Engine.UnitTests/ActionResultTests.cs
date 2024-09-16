using scienide.Engine.Game;

namespace scienide.Engine.UnitTests;

public class ActionResultTests
{
    [Fact]
    public void Creation_Success_TestSuccess()
    {
        var successResult = GetSuccessResult();
        Assert.True(successResult.Succeeded);
    }

    private ActionResult GetSuccessResult() => new ActionResult(true, true);
}
