﻿namespace scienide.Engine.Components;

using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

internal abstract class BehaviourBase(IActor actor) : GameComponent
{
    public IActor Actor { get; set; } = actor;

    public abstract void EvaluateState();

    public abstract IActionCommand Act();
}