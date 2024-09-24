using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game;
using scienide.Engine.Game.Actions;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.UnitTests;

public class CircularListTests
{
    private class TestTimedEntity : TimedEntity
    {
        private Action Action;

        public TestTimedEntity(/*IGameComponent component,*/ Action a) : base(null)
        {
            Action = a;
        }

        public override IActionCommand TakeTurn()
        {
            Action();
            return new RestAction();
        }
    }

    private class MoveLeftAction : ActionCommand
    {
        private const string ActionName = "Move to the left";
        private const string ActionDescr = "{0} moved to the left.";
        private const int Cost = 100;


        public MoveLeftAction(IActor actor)
            : base(ActionName, ActionDescr/*, actor*/)
        {
        }

        public override int Execute()
        {
            // Actor.Position += Direction.Left;
            return Cost;
        }

        public override void Undo()
        {
            // move to the right
        }
    }

    [Fact]
    public void ExecuteNode_ShouldRunForNode()
    {
        var map = new GameMap(10, 10);
        var actorTimeEntity = new TestTimedEntity(() => Assert.Fail())
        {
            Speed = 100,
            Cost = 100,
            Energy = -200
        };
        var actorTimeEntity2 = new TestTimedEntity(() => { })
        {
            Speed = 101,
            Cost = 100,
            Energy = -200
        };
        map.AddChild(new Actor((1, 2), new Glyph('@', (1, 2)), actorTimeEntity));
        map.AddChild(new Actor((2,1), new Glyph('I', (2,1)), actorTimeEntity2));

        var circularList = new CircularList();
        circularList.Add(actorTimeEntity);
        circularList.Add(actorTimeEntity2);

        while (true)
        {
            circularList.ProgressSentinel();
            circularList.ProgressTime();
        }
    }
}
