using scienide.Engine.Core.Interfaces;
using scienide.Engine.Game.Actions;

namespace scienide.Engine.Game.TimedEntities;

public class HeroEntity : TimedEntity
{
    public override IActionCommand TakeTurn(IActor actor)
    {
        // The idea is to process input here and create the action
        // that needs to be executed

        return new RestAction();
    }
}
