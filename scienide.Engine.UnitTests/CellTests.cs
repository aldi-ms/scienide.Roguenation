using scienide.Engine.Core;

namespace scienide.Engine.UnitTests;

public class CellTests
{
    [Fact]
    public void AddChild_ChildIsValid_ShouldBeVisibleThroughChildren()
    {
        var gObj = new Cell();
        var cell = new Cell();

        Assert.True(cell.AddChild(gObj));
        Assert.Contains(gObj, cell.Children);
    }

    [Fact]
    public void AddChild_ChildIsNotValid_ShouldReturnFalse()
    {
        GameComponent? gObj = null;
        var cell = new Cell();

#pragma warning disable CS8604 // Possible null reference argument.
        Assert.False(cell.AddChild(gObj));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public void AddChild_ChildAlreadyExists_ShouldReturnFalse()
    {
        var gObj = new Cell();
        var cell = new Cell();
        cell.AddChild(gObj);

        Assert.False(cell.AddChild(gObj));
        Assert.Contains(gObj, cell.Children);
    }

    [Fact]
    public void RemoveChild_ChildExistsInCell_ShouldBeRemovedFromChildren()
    {
        var gObj = new Cell();
        var cell = new Cell();
        cell.AddChild(gObj);

        Assert.True(cell.RemoveChild(gObj));
        Assert.DoesNotContain(gObj, cell.Children);
    }

    [Fact]
    public void RemoveChild_ChildDoesNotExistInCell_ShouldReturnFalse()
    {
        var gObj = new Cell();
        var cell = new Cell();

        Assert.False(cell.RemoveChild(gObj));
    }

    [Fact]
    public void RemoveChild_ChildNotValid_ShouldReturnFalse()
    {
        GameComponent? gObj = null;
        var cell = new Cell();

#pragma warning disable CS8604 // Possible null reference argument.
        Assert.False(cell.RemoveChild(gObj));
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
