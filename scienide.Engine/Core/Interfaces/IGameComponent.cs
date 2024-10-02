﻿using SadRogue.Primitives;
using scienide.Engine.Game;

namespace scienide.Engine.Core.Interfaces;

/// <summary>
/// The base component object in the hierarchy.
/// </summary>
public interface IGameComponent
{
    /// <summary>
    /// The (X; Y) coordinates of the cell if it is part of a larger map
    /// </summary>
    public Point Position { get; set; }

    IGameComponent? Parent { get; set; }

    Glyph Glyph { get; set; }

    GameObjectType ObjectType { get; protected set; }

    void Traverse(Action<IGameComponent> action);
}
