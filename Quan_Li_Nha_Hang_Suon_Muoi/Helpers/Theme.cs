using System.Drawing;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
public static class Theme
{
    public static readonly Color Brick = ColorTranslator.FromHtml("#A83F2B");
    public static readonly Color BrickDark = ColorTranslator.FromHtml("#85301F");
    public static readonly Color Charcoal = ColorTranslator.FromHtml("#24201D");
    public static readonly Color Cream = ColorTranslator.FromHtml("#FFF7ED");
    public static readonly Color Sand = ColorTranslator.FromHtml("#F5EBDD");
    public static readonly Color Green = ColorTranslator.FromHtml("#28764C");
    public static readonly Color White = Color.White;
    public static readonly Color Muted = ColorTranslator.FromHtml("#82766D");
    public static readonly Color Border = ColorTranslator.FromHtml("#E7DCCF");
    public static readonly Color SoftRed = ColorTranslator.FromHtml("#F8E8E3");
    public static Button MakeButton(string text, bool primary = true)
    {
        var button = new Button
        {
            Text = text,
            Height = 42,
            AutoSize = false,
            FlatStyle = FlatStyle.Flat,
            BackColor = primary ? Brick : White,
            ForeColor = primary ? White : Charcoal,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false,
            Margin = new Padding(0)
        };
        button.FlatAppearance.BorderSize = primary ? 0 : 1;
        button.FlatAppearance.BorderColor = Border;
        button.FlatAppearance.MouseOverBackColor = primary ? BrickDark : Sand;
        button.FlatAppearance.MouseDownBackColor = BrickDark;
        return button;
    }

    public static Label MakeLabel(string text, float size, bool bold = false, Color? color = null)
    {
        return new()
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = color ?? Charcoal,
            BackColor = Color.Transparent
        };
    }
}
