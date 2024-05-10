using System;
using System.Drawing;
using QuantowerPlugin_Decomplied;
using Services;

namespace Front_End.Renderables;

public class Text_Renderable : IRenderable
{
    public Rectangle Bounds { get; set; }
    public UI_usage B_Usage { get; set; } = UI_usage.Simple_Text;
    public int Min_Widt { get; private set; } = 10;
    public int Max_Widt { get; private set; }
    public string Text { get; private set; }

    public Text_Renderable(Rectangle bounds, string text, int max_width = 90)
    {
        Bounds = bounds;
        Max_Widt = max_width;
        Text = text;
    }

    public void Draw(Graphics graphics)
    {
        SizeF textSize = graphics.MeasureString(Text, Theme_Plug.Font);
        PointF textPosition = new PointF(Bounds.X + (Bounds.Width - textSize.Width) / 2, Bounds.Y + (Bounds.Height - textSize.Height) / 2);

        // Crea un rettangolo di disegno che limita il testo all'interno dei Bounds
        RectangleF textRect = new RectangleF(
            Bounds.X,
            Bounds.Y,
            Bounds.Width,
            Bounds.Height
        );

        // Utilizza il rettangolo di clipping per disegnare il testo
        // Questo assicura che il testo non esci dai confini definiti
        graphics.SetClip(textRect);

        graphics.DrawString(Text, Theme_Plug.Font, Theme_Plug.Text_brush, textPosition);

        graphics.ResetClip();
    }
    public Size GetPreferredSize(Graphics graphics) => throw new NotImplementedException();
    public void On_Click() => throw new NotImplementedException();
    public void SetBounds(Rectangle bounds)
    {
        if (bounds.Width < Min_Widt)
            bounds.Width = Min_Widt;

        if (bounds.Width > Max_Widt)
            bounds.Width = Max_Widt;

        Bounds = bounds;
    }
}
