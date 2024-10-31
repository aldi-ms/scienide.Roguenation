namespace WFCSampleGenerator;

using SadConsole;
using SadConsole.Input;
using SadConsole.Quick;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using System.Diagnostics;
using System.Text;
using Keyboard = SadConsole.Input.Keyboard;

internal class RootScreen : ScreenObject
{
    private static char LeftClickCharBrush = '#';
    private static char RightClickCharBrush = ' ';
    private static bool _shouldDelete = false;

    private readonly ControlsScreen _controlsScreen;
    private readonly Dictionary<Point, ColoredGlyphBase> _oldGlyphs;

    private int _regionSizeX = -1;
    private int _regionSizeY = -1;
    private ScreenSurface _drawScreenSurface;

    public RootScreen()
    {
        _oldGlyphs = new Dictionary<Point, ColoredGlyphBase>();
        _drawScreenSurface = new ScreenSurface(1, 1);
        _controlsScreen = new ControlsScreen(GameSettings.CONTROLS_WIDTH, GameSettings.CONTROLS_HEIGHT)
        {
            UseMouse = true,
            UseKeyboard = true,
            FocusOnMouseClick = true,
            FocusedMode = FocusBehavior.Set,
            Position = GameSettings.ControlsScreenRect.Position
        };

        Border.CreateForSurface(_controlsScreen, "Controls");

        Children.Add(_controlsScreen);

        _controlsScreen.ApplyButton.Click += ApplyButton_Click;
        _controlsScreen.LeftClickBrush.TextChanged += LeftClickBrush_TextChanged;
        _controlsScreen.RightClickBrush.TextChanged += RightClickBrush_TextChanged;
        _controlsScreen.DeleteButton.Click += DeleteButton_Click;

        InitDrawScreen();
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        var button = sender as Button ?? throw new ArgumentNullException(nameof(sender));
        _shouldDelete = !_shouldDelete;
        button.Text = _shouldDelete ? button.Text.ToUpper() : button.Text.ToLower();
    }

    private void RightClickBrush_TextChanged(object? sender, EventArgs e)
    {
        var tb = sender as TextBox ?? throw new ArgumentNullException(nameof(sender));
        if (tb.Text.Length == 1)
        {
            RightClickCharBrush = tb.Text[0];
        }
    }

    private void LeftClickBrush_TextChanged(object? sender, EventArgs e)
    {
        var tb = sender as TextBox ?? throw new ArgumentNullException(nameof(sender));
        if (tb.Text.Length == 1)
        {
            LeftClickCharBrush = tb.Text[0];
        }
    }

    private void ApplyButton_Click(object? sender, EventArgs e)
    {
        InitDrawScreen();
    }

    private void InitDrawScreen()
    {
        _oldGlyphs.Clear();
        if (_drawScreenSurface != null)
        {
            Children.Remove(_drawScreenSurface);
        }

        var screenWidth = int.Parse(_controlsScreen.WidthBox.Text);
        var screenHeight = int.Parse(_controlsScreen.HeightBox.Text);
        _drawScreenSurface = new ScreenSurface(screenWidth, screenHeight)
        {
            UseMouse = true,
            UseKeyboard = true,
            FocusOnMouseClick = true,
            FocusedMode = FocusBehavior.Set,
            Position = new Point(_controlsScreen.Width + 6, 2),
        };

        Border.CreateForSurface(_drawScreenSurface, "Draw");

        _drawScreenSurface.WithMouse(ProcessMouse);
        _drawScreenSurface.WithKeyboard(ProcessInput);

        _regionSizeX = int.Parse(_controlsScreen.RegionSizeX.Text);
        _regionSizeY = int.Parse(_controlsScreen.RegionSizeY.Text);
        var regionCountX = screenWidth / _regionSizeX;
        var regionCountY = screenHeight / _regionSizeY;
        var regionColorMap = new Dictionary<Point, Color>();

        bool changeColor = false;
        for (var x = 0; x < regionCountX; x++)
        {
            if (regionCountY % 2 == 0)
            {
                changeColor = !changeColor;
            }

            for (var y = 0; y < regionCountY; y++)
            {
                var globalY = y * _regionSizeY;
                var globalX = x * _regionSizeX;

                var backgroundColor = changeColor ? Color.DimGray : Color.Gray;
                var shapeParams = ShapeParameters.CreateFilled(new ColoredGlyph(Color.White, backgroundColor, ' ')
                    , new ColoredGlyph(Color.White, backgroundColor, ' '));
                _drawScreenSurface.DrawBox(new Rectangle(globalX, globalY, _regionSizeX, _regionSizeY), shapeParams);
                changeColor = !changeColor;
            }
        }

        Children.Add(_drawScreenSurface);
    }

    private bool ProcessMouse(IScreenObject screenObject, MouseScreenObjectState state)
    {
        if (state.Mouse.LeftClicked && _drawScreenSurface.IsFocused)
        {
            return ProcessMouseClick(new Point(state.CellPosition.X, state.CellPosition.Y), LeftClickCharBrush);
        }

        if (state.Mouse.RightClicked && _drawScreenSurface.IsFocused)
        {
            return ProcessMouseClick(new Point(state.CellPosition.X, state.CellPosition.Y), RightClickCharBrush);
        }

        return false;
    }

    private bool ProcessMouseClick(Point cellPos, char ch)
    {
        if (!_oldGlyphs.TryGetValue(cellPos, out ColoredGlyphBase? value))
        {
            var oldAppearance = _drawScreenSurface.GetCellAppearance(cellPos.X, cellPos.Y);
            value = oldAppearance;
            _oldGlyphs.Add(cellPos, value);
        }

        if (_shouldDelete)
        {
            _drawScreenSurface.SetCellAppearance(cellPos.X, cellPos.Y, value);
        }
        else
        {
            _drawScreenSurface.SetCellAppearance(cellPos.X, cellPos.Y,
                new ColoredGlyph(Color.White, value.Background, ch));
        }

        return true;
    }

    private bool ProcessInput(IScreenObject screenObject, Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.S))
        {
            Trace.WriteLine("Save!");
            SaveSampleWFCFile($@"..\..\..\..\scienide.WaveFunctionCollapse\inputs\input-{_regionSizeX}x{_regionSizeY}-{DateTime.Now:yyyy-MM-dd_HH-mm}.in");
            return true;
        }

        if (keyboard.IsKeyPressed(Keys.OemQuestion))
        {
            Trace.WriteLine("Help!");
        }

        if (keyboard.IsKeyPressed(Keys.Q))
        {
            Environment.Exit(0);
        }

        return false;
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (_drawScreenSurface.ProcessKeyboard(keyboard))
        {
            return true;
        }

        if (_controlsScreen.ProcessKeyboard(keyboard))
        {
            return true;
        }

        return base.ProcessKeyboard(keyboard);
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        if (_drawScreenSurface.IsFocused)
        {
            if (_drawScreenSurface.ProcessMouse(state))
            {
                return true;
            }
            return false;
        }

        if (_controlsScreen.IsFocused)
        {
            if (_controlsScreen.ProcessMouse(state))
            {
                return true;
            }
            return false;
        }

        var result = base.ProcessMouse(state);
        return result;
    }

    private void SaveSampleWFCFile(string fileName)
    {
        using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.None);
        using var sw = new StreamWriter(fs, Encoding.ASCII);

        for (int y = 0; y < _drawScreenSurface.Height; y++)
        {
            for (int x = 0; x < _drawScreenSurface.Width; x++)
            {
                sw.Write(_drawScreenSurface.Surface[x, y].GlyphCharacter);
            }

            if (y < _drawScreenSurface.Height - 1)
                sw.Write(Environment.NewLine);
        }
    }
}
