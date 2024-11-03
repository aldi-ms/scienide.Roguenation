namespace WFCSampleGenerator;

using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

internal class ControlsScreen : ControlsConsole
{
    private const string WhiteSpace = " ";

    public Button ResetButton { get; private set; }
    public Button DeleteButton { get; private set; }

    public NumberBox WidthBox;
    public NumberBox HeightBox;
    public NumberBox RegionSizeX;
    public NumberBox RegionSizeY;

    public TextBox LeftClickBrush;
    public TextBox RightClickBrush;

    public ControlsScreen(int width, int height) : base(width, height)
    {
        Controls.ThemeColors = Colors.CreateAnsi();

        #region Prints
        var texts = new string[]
        {
                "<- Pixel Width (X)",
                "<- Pixel Height (Y)",
                "<- Region Width (X)",
                "<- Region Height (Y)",
                "<- Left MB Brush",
                "<- Right MB Brush",
        };

        for (int i = 0; i < texts.Length; i++)
        {
            this.Print(6, i, texts[i], Controls.ThemeColors.YellowDark);
        }

        #endregion

        #region Controls creation

        WidthBox = new NumberBox(6)
        {
            DefaultValue = GameSettings.DrawScreenWidth,
            Position = Point.Zero,
            AllowDecimal = true,
            ShowUpDownButtons = true,
            Text = GameSettings.DrawScreenWidth.ToString()
        };

        HeightBox = new NumberBox(6)
        {
            DefaultValue = GameSettings.DrawScreenHeight,
            Position = new Point(0, 1),
            AllowDecimal = true,
            ShowUpDownButtons = true,
            Text = GameSettings.DrawScreenHeight.ToString()
        };

        RegionSizeX = new NumberBox(6)
        {
            DefaultValue = 3,
            Position = new Point(0, 2),
            AllowDecimal = true,
            ShowUpDownButtons = true,
            Text = "3"
        };

        RegionSizeY = new NumberBox(6)
        {
            DefaultValue = 3,
            Position = new Point(0, 3),
            AllowDecimal = true,
            ShowUpDownButtons = true,
            Text = "3"
        };

        LeftClickBrush = new TextBox(2)
        {
            Position = new Point(4, 4),
            Validator = new StringValidation.Validator(StringValidation.LengthRange(0, 1, "Text should be 1 char max"))
        };
        LeftClickBrush.TextValidated += LeftClickBrush_TextValidated;

        RightClickBrush = new TextBox(2)
        {
            Position = new Point(4, 5),
            Validator = new StringValidation.Validator(StringValidation.LengthRange(0, 1, "Text should be 1 char max"))
        };
        RightClickBrush.TextValidated += RightClickBrush_TextValidated;

        DeleteButton = new Button("delete")
        {
            Position = new Point(Width - 11, Height - 2)
        };

        ResetButton = new Button("Reset")
        {
            Position = new Point(Width - 10, Height - 1)
        };
        #endregion

        Controls.Add(WidthBox);
        Controls.Add(HeightBox);
        Controls.Add(RegionSizeX);
        Controls.Add(RegionSizeY);
        Controls.Add(ResetButton);
        Controls.Add(DeleteButton);
        Controls.Add(LeftClickBrush);
        Controls.Add(RightClickBrush);
    }

    private void RightClickBrush_TextValidated(object? sender, StringValidation.Result e)
    {
        if (!e.IsValid)
        {
            var tb = sender as TextBox ?? throw new ArgumentNullException(nameof(sender));
            tb.Text = WhiteSpace;
        }
    }

    private void LeftClickBrush_TextValidated(object? sender, StringValidation.Result e)
    {
        if (!e.IsValid)
        {
            var tb = sender as TextBox ?? throw new ArgumentNullException(nameof(sender));
            tb.Text = WhiteSpace;
        }
    }
}
