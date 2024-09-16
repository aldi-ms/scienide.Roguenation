using scienide.Engine.Game;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.UnitTests;

public class CircularListTests
{
    [Fact]
    public void ExecuteNode_ShouldRunForNode()
    {
        var circularList = new CircularList();
        circularList.Add(new DefaultEntity
        {
            Speed = 100,
            Cost = 100,
            Energy = -200
        });
        circularList.Add(new DefaultEntity
        {
            Energy = -200,
            Speed = 102,
            Cost = 100,
        });

        while (true)
        {
            circularList.ProgressSentinel();
            circularList.ProgressTime();
        }
    }
}
