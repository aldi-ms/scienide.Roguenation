﻿namespace scienide.Engine.UnitTests;

using scienide.Common.Game;
using scienide.Common.Infrastructure;
using scienide.Engine.Game;

public class GameMapTests
{
    [Fact]
    public void Construct_Default_CheckCorrectConstruction()
    {
        var map = new GameMap(null, new FlatArray<Glyph>(1, 1), true);
        var firstElement = map.Data.FirstOrDefault();

        Assert.NotNull(firstElement);
        Assert.NotNull(firstElement.Parent);
        Assert.Equal(map, firstElement.Parent);
    }
}
