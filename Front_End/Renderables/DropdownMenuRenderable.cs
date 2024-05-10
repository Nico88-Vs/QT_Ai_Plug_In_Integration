using System;
using System.Collections.Generic;
using System.Drawing;
using Services;
using Services.Command;

namespace Front_End.Renderables
{
    public class DropdownMenuRenderable : IRenderable
    {
        public List<string> Items { get; private set; }
        public int SelectedIndex { get; private set; } = -1;
        public Rectangle Bounds { get; set; }
        public UI_usage B_Usage { get; set; }

        public List<Rectangle> Items_Bounds { get; set; }

        public int Min_Widt { get; } = 10;
        public int Max_Widt { get; } = 120;

        public bool isDroppedDown = false;

        public event EventHandler<EventArgsCommand> Clicked;

        public DropdownMenuRenderable(UI_usage b_Usage, Rectangle bounds)
        {
            B_Usage = b_Usage;
            SetBounds(bounds);
            Items = new List<string>();
            Items_Bounds = new List<Rectangle>();
        }

        public void Draw(Graphics graphics)
        {
            // Disegna lo sfondo del dropdown
            graphics.FillRectangle(Theme_Plug.Brash_Backcground_Button, Bounds);
            graphics.DrawRectangle(Theme_Plug.BorderPen, Bounds);

            //// Disegna il testo dell'elemento selezionato o il testo predefinito
            string textToDisplay = SelectedIndex >= 0 ? Items[SelectedIndex] : "Select...";
            SizeF textSize = graphics.MeasureString(textToDisplay, Theme_Plug.Font);
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

            graphics.DrawString(textToDisplay, Theme_Plug.Font, Theme_Plug.Text_brush, textPosition);

            // Disegna l'icona del dropdown (se vuoi usare un PNG)
            Image dropdownIcon = Image.FromFile(@"C:\Users\user\source\repos\Ai_Integration_Plugin\Resources\noun-down-arrow-down-1144832_done.png");
            graphics.DrawImage(dropdownIcon, new Rectangle(Bounds.Right - 20, Bounds.Y + 5, 15, Bounds.Height - 10));

            graphics.ResetClip();


            // Se il menu è aperto, disegna gli elementi del menu
            if (isDroppedDown)
            {
                Items_Bounds.Clear();
                int yOffset = Bounds.Bottom;
                foreach (string item in Items)
                {
                    Rectangle itemBounds = new Rectangle(Bounds.X, yOffset, Bounds.Width, 30);
                    Items_Bounds.Add(itemBounds);
                    graphics.FillRectangle(Brushes.LightGray, itemBounds);
                    graphics.DrawString(item, Theme_Plug.Font, Brushes.Black, new PointF(Bounds.X + 10, yOffset + 5));
                    yOffset += 30;
                }
            }
        }

        public void ToggleDropdown()
        {
            isDroppedDown = !isDroppedDown;

            if (isDroppedDown == false)
                Items_Bounds.Clear();
        }

        public void AddItem(string item)
        {
            Items.Add(item);
        }

        public void SelectItem(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                SelectedIndex = index;
                ToggleDropdown(); // Optional: close the dropdown when an item is selected
            }
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
        public void On_Click() => ToggleDropdown();

        // Metodi aggiuntivi come SetBounds e GetPreferredSize, se necessari
    }
}
