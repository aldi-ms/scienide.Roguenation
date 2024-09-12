using scienide.Engine.Core.Interfaces;

namespace scienide.Engine.Game;

public class TimedEntity : ITimedEntity
{
    public int Energy { get; set; }
    public int Speed { get; set; }
    public int Cost { get; set; }
}
