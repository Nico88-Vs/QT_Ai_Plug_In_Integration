using System;
using System.Drawing;
using QuantowerPlugin_Decomplied;
using Services;
using Services.Command;

namespace Front_End.Renderables;

internal class Images_Renderable : IRenderable
{
    public Rectangle Bounds { get; set; }
    public UI_usage B_Usage { get; set; }

    public int Min_Widt { get; private set; } = 30;

    public int Max_Widt { get; private set; } = 30;

    public string Reference_Path { get; private set; }

    public Images_Renderable(string reference_Path, Rectangle bounds, UI_usage usage = UI_usage.Image)
    {
        if (usage == UI_usage.Logo)
        {
            Max_Widt = Min_Widt = 60;
            B_Usage = usage;
        }
        else
            B_Usage = UI_usage.Image;

        SetBounds(bounds);

        Reference_Path = reference_Path;
    }

    public event EventHandler<EventArgsCommand> Clicked;

    public void Draw(Graphics gr)
    {
        Image image = Image.FromFile(Reference_Path);
        Rectangle destRect = Bounds;

        #region maschera
        ////Crea una ColorMatrix e applica il colore della maschera
        //float r = 27 / 255f;
        //float g = 43 / 255f;
        //float b = 50 / 255f;

        //// Calcola il nuovo rapporto di aspetto per adattarsi a Unit_Size mantenendo il rapporto di aspetto originale
        //float scaleFactor = Math.Min((float)Unit_Size.Width / image.Width, (float)Unit_Size.Height / image.Height);
        //int scaledWidth = (int)(image.Width * scaleFactor);
        //int scaledHeight = (int)(image.Height * scaleFactor);

        //// Crea il rettangolo centrato se necessario
        //int offsetX = (Unit_Size.Width - scaledWidth) / 2;
        //int offsetY = (Unit_Size.Height - scaledHeight) / 2;
        //Rectangle destRect = new Rectangle(offsetX, offsetY, scaledWidth, scaledHeight);


        //ColorMatrix colorMatrix = new ColorMatrix(
        //    new float[][]
        //    {
        //        new float[] {0, 0, 0, 0, 0},
        //        new float[] {0, 0, 0, 0, 0},
        //        new float[] {0, 0, 0, 0, 0},
        //        new float[] {0, 0, 0, 1, 0},
        //        new float[] {r, g, b, 0, 1}
        //    });

        //ImageAttributes attributes = new ImageAttributes();
        //attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        #endregion

        //Disegna l'immagine originale con la matrice colore applicata
        //gr.DrawImage(image, new Rectangle(0, 0, this.Unit_Size.Width, this.Unit_Size.Height), 0, 0, Unit_Size.Width, Unit_Size.Height, GraphicsUnit.Pixel, attributes);
        gr.DrawImage(image, Bounds);
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
