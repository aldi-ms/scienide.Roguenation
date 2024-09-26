using scienide.Engine.Core;
using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game;
using scienide.Engine.Game.Actions;
using scienide.Engine.Game.Actors;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.UnitTests;

public class CircularListTests
{
    //private class TestTimedEntity : TimedEntity
    //{
    //    private Action Action;

    //    public TestTimedEntity(/*IGameComponent component,*/ Action a) : base()
    //    {
    //        Action = a;
    //    }

    //    public override IActionCommand TakeTurn(IActor actor)
    //    {
    //        Action();
    //        return new RestAction(actor);
    //    }
    //}

    private class MoveLeftAction : ActionCommand
    {
        public MoveLeftAction(IActor actor)
            : base(actor, 100,"Move to the left", "{0} moved to the left."/*, actor*/)
        {
        }

        public override int Execute()
        {
            // Actor.Position += Direction.Left;
            return Cost;
        }

        public override string GetActionLog()
        {
            return string.Format(Description, Actor?.Name ?? "The actor");
        }

        public override void Undo()
        {
            // move to the right
        }
    }

    [Fact]
    public void ExecuteNode_ShouldRunForNode()
    {
        var circularList = new CircularList();
        circularList.Add(new Hero(string.Empty, (1, 2), new Glyph('@', (1, 2))));
        circularList.Add(new Monster(string.Empty, (2, 1), new Glyph('I', (2, 1))));

        while (true)
        {
            circularList.ProgressSentinel();
            circularList.ProgressTime();
        }
    }
}
