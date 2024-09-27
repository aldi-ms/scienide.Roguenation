using scienide.Engine.Game;

namespace scienide.Engine.UnitTests;
public class GameMapTests
{
    [Fact]
    public void Construct_Default_CheckCorrectConstruction()
    {
        var map = new GameMap(1, 1);
        var firstElement = map.Data.FirstOrDefault();

        Assert.NotNull(firstElement);
        Assert.NotNull(firstElement.Parent);
        Assert.Equal(map, firstElement.Parent);
    }

    [Fact]
    public void TraverseMap_Default_CheckFullTraversal()
    {
        var map = new GameMap(10, 10);

        map.Traverse(x => x.Parent = null);

        foreach (var cell in map.Data)
        {
            Assert.Null(cell.Parent);
        }
    }

    [Fact]
    public void Construct_AndRemoveDataFromMap_EnsureMapDataEmpty()
    {
        var map = new GameMap(100, 100);
        for (var x = 0; x < 100; x++)
        {
            for (var y = 0; y < 100; y++)
            {
                map.RemoveChild(map.Data[x, y]);
            }
        }

        Assert.True(map.Data.Count == 0);
        Assert.True(map.Children.Count == 0);
    }
}
