using SadRogue.Primitives;
using scienide.Common.Game;

namespace scienide.Engine.UnitTests;

public class CellTests
{
    [Fact]
    public void AddChild_ChildIsValid_ShouldBeVisibleThroughChildren()
    {
        var gObj = CreateNewCell();
        var cell = CreateNewCell();

        Assert.True(cell.AddComponent(gObj));
        Assert.Contains(gObj, cell.Components);
    }

    [Fact]
    public void AddChild_ChildIsNotValid_ShouldReturnFalse()
    {
        GameComponent? gObj = null;
        var cell = CreateNewCell();

        Assert.False(cell.AddComponent(gObj));
    }

    [Fact]
    public void AddChild_ChildAlreadyExists_ShouldReturnFalse()
    {
        var gObj = CreateNewCell();
        var cell = CreateNewCell();
        cell.AddComponent(gObj);

        Assert.False(cell.AddComponent(gObj));
        Assert.Contains(gObj, cell.Components);
    }

    [Fact]
    public void RemoveChild_ChildExistsInCell_ShouldBeRemovedFromChildren()
    {
        var gObj = CreateNewCell();
        var cell = CreateNewCell();
        cell.AddComponent(gObj);

        Assert.True(cell.RemoveComponent(gObj));
        Assert.DoesNotContain(gObj, cell.Components);
    }

    [Fact]
    public void RemoveChild_ChildDoesNotExistInCell_ShouldReturnFalse()
    {
        var gObj = CreateNewCell();
        var cell = CreateNewCell();

        Assert.False(cell.RemoveComponent(gObj));
    }

    [Fact]
    public void RemoveChild_ChildNotValid_ShouldReturnFalse()
    {
        GameComponent? gObj = null;
        var cell = CreateNewCell();

#pragma warning disable CS8604 // Possible null reference argument.
        Assert.False(cell.RemoveComponent(gObj));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    private static Cell CreateNewCell() => new Cell(Point.None);
}
