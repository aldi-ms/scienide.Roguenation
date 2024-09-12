using SadRogue.Primitives;
using scienide.Engine.Core;
using scienide.Engine.Infrastructure;

namespace scienide.Engine.Game;

public class GameMap : GameComposite
{
    private FlatArray<GameComposite> _data;

    public GameMap(int width, int height)
    {
        _data = new FlatArray<GameComposite>(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = CellBuilder.CreateBuilder()
                    .WithLocation(new Point(x, y))
                    .Build();

                _data[x, y] = cell;
                AddChild(cell);
            }
        }
    }

    public FlatArray<GameComposite> Data => _data;
}
