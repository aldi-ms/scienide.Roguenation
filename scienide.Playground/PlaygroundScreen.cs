﻿namespace scienide.Playground;

using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Engine.Game;
using System.Diagnostics;

internal class PlaygroundScreen : GameScreenBase
{
    public PlaygroundScreen(int width, int height)
        : base(width, height, new Point(1, 1), MapGenerationStrategy.Empty, string.Empty)
    {
    }

    public override bool HandleMouseState(IScreenObject screenObject, MouseScreenObjectState state)
    {
        if (state.Mouse.LeftClicked)
        {
            var selectedCell = Map[state.CellPosition];
            switch (selectedCell.Glyph.Char)
            {
                case ' ':
                case '.':
                    selectedCell.Terrain = new Terrain(new Glyph('#'));
                    break;
                case '#':
                    selectedCell.Terrain = new Terrain(new Glyph('.'));
                    break;
                default:
                    Trace.WriteLine($"Unknown option, glyph was: {selectedCell.Glyph}.");
                    break;
            }

            Map.DirtyCells.Add(selectedCell);
            return true;
        }

        return false;
    }
}
