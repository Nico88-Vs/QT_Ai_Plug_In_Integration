using System;
using System.Drawing;
using Ai_Integration_Plugin_due;
using Services;
using Services.Command;
namespace Front_End.Renderables
{
    public class ButtonRenderable : IRenderable
    {
        public string Text { get; set; }
        public Rectangle Bounds { get; set; }
        public UI_usage B_Usage { get; set; }
        public int Min_Widt { get; private set; } = 10;
        public int Max_Widt { get; private set; } = 60;

        public event EventHandler<EventArgsCommand> Clicked;

        public ButtonRenderable(UI_usage b_Usage, Rectangle bounds)
        {
            Text = b_Usage.ToString();
            B_Usage = b_Usage;

            if (B_Usage == UI_usage.Lunch_Python)
            {
                Min_Widt = 3 * Min_Widt;
                Max_Widt = 3 * Max_Widt;
            }

            SetBounds(bounds);
        }

        public void Draw(Graphics graphics)
        {
            //graphics.Clear(Theme.BackgroundColor);

            // Disegna lo sfondo del bottone
            graphics.FillRectangle(Theme_Plug.Brash_Backcground_Button, Bounds);
            graphics.DrawRectangle(Theme_Plug.BorderPen, Bounds);

            // Disegna il testo del bottoneb
            SizeF textSize = graphics.MeasureString(Text, Theme_Plug.Font);
            PointF textPosition = new PointF(Bounds.X + (Bounds.Width - textSize.Width) / 2,
                                             Bounds.Y + (Bounds.Height - textSize.Height) / 2);

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

        public virtual void OnClick()
        {
            EventArgsCommand args = new EventArgsCommand(this);
            Clicked?.Invoke(this, args);
        }

        public void SetBounds(Rectangle bounds)
        {
            if (bounds.Width < Min_Widt)
                bounds.Width = Min_Widt;

            if (bounds.Width > Max_Widt)
                bounds.Width = Max_Widt;

            Bounds = bounds;
        }
        public Size GetPreferredSize(Graphics graphics) => throw new NotImplementedException();
    }

}
