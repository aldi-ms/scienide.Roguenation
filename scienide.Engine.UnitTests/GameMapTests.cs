﻿namespace scienide.Engine.UnitTests;

using scienide.Engine.Game;

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
}
