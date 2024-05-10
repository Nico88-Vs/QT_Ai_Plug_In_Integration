using Front_End.Renderables;
using Front_End;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using TradingPlatform.BusinessLayer;
using TradingPlatform.BusinessLayer.Native;
using TradingPlatform.PresentationLayer.Plugins;
using TradingPlatform.PresentationLayer.Renderers;
using Services;

namespace Ai_Integration_Plugin
{
    public class GdiRenderer : Renderer
    {
        #region Proeprties
        private readonly BufferedGraphic bufferedGraphic;
        public List<IRenderable> renderables { get; set; }
        public LinearLayoutManager Layout_Manager { get; set; }
        private Size _size;
        public Size Unit_Size { get => this._size; }
        public Symbol Current_Symbol { get; set; }
        public Indicator Current_indicator { get; set; }
        public Pages CurrentPage { get; set; } = Pages.Run;
        #endregion

        public GdiRenderer(IRenderingNativeControl native, Size unit_size, Symbol current_Symbol, Indicator current_indicator)
           : base(native)
        {
            this._size = unit_size;
            this.renderables = new List<IRenderable>();

            // Subscribe to mouse events
            native.MouseDownNative += OnMouseDown;
            native.MouseUpNative += OnMouseUp;
            native.MouseMoveNative += OnMouseMove;

            bufferedGraphic = new BufferedGraphic(Draw, Refresh, native.DisposeImage, native.IsDisplayed, requiredThreadType: BufferedGraphicRequiredThreadType.LowPriority);

            this.Current_Symbol = current_Symbol;
            this.Current_indicator = current_indicator;
            this.Layout_Manager = LinearLayoutManager.Instance;
            this.Layout_Manager.Inizialize(this.Current_Symbol, this.Current_indicator, this._size);
        }

        public void Update_Lay(Symbol sy, Indicator ind)
        {
            this.Current_indicator = ind;
            this.Current_Symbol = sy;
            // TODO: eventuale problema d'aggiornato della pagiana corrente
            this.Layout_Manager.Inizialize(this.Current_Symbol, this.Current_indicator, this.Unit_Size);
            this.RedrawBufferedGraphic();
        }

        public void Set_size(Size size)
        {
            this._size = size;
            this.Layout_Manager.Inizialize(this.Current_Symbol, this.Current_indicator, this.Unit_Size);
            this.RedrawBufferedGraphic();
        }

        public void RedrawBufferedGraphic()
        {
            bufferedGraphic.IsDirty = true;
        }

        protected virtual void Draw(Graphics gr)
        {
            this.Drow_Bg(gr);

            foreach (IRenderable renderable in Layout_Manager.controls)
            {
                renderable.Draw(gr);
            }
        }

        public override nint Render() => bufferedGraphic.CurrentImage;

        public override void Dispose()
        {
            if (bufferedGraphic != null)
                bufferedGraphic.Dispose();

            base.Dispose();
        }

        public override void OnResize()
        {
            base.OnResize();

            #region old may qt resizing
            Rectangle bounds = Bounds;
            if (bounds.Width == 0 || bounds.Height == 0)
                return;

            this.Set_size(new Size(bounds.Width, bounds.Height));
            this.Layout_Manager.Resize(this.Unit_Size);

            try
            {
                bufferedGraphic.Resize(bounds.Width, bounds.Height);
                bufferedGraphic.IsDirty = true;
            }
            catch (Exception ex)
            { Console.WriteLine("Error during resizing: " + ex.Message); }
            #endregion
        }

        #region Mouse Processing
        private void OnMouseDown(NativeMouseEventArgs e)
        {
            // Add your MouseDown processing logic here
        }

        private void OnMouseMove(NativeMouseEventArgs obj)
        {
            // Add your MouseMove processing logic here
        }

        private void OnMouseUp(NativeMouseEventArgs obj)
        {
            foreach (var renderable in Layout_Manager.controls)
            {
                if (renderable.Bounds.Contains(obj.Location))
                {
                    //if (renderable.B_Usage == UI_usage.Drop_Down)
                    //{
                    //    renderable.On_Click();
                    //    this.RedrawBufferedGraphic();
                    //}

                    if (renderable.B_Usage == UI_usage.Lunch_Python)
                    {
                        this.CurrentPage =Pages.Started;
                        this.Layout_Manager.Switch_Page(Pages.Started);
                        this.RedrawBufferedGraphic();
                    }
                }
            }

            try
            {
                List<DropdownMenuRenderable> dd_list = Layout_Manager.controls.OfType<DropdownMenuRenderable>().ToList();
                foreach (DropdownMenuRenderable dropdown in dd_list)
                {
                    if (dropdown.isDroppedDown)
                    {
                        foreach (Rectangle rect in dropdown.Items_Bounds)
                        {
                            if (rect.Contains(obj.Location))
                            {
                                dropdown.SelectItem(dropdown.Items_Bounds.IndexOf(rect));
                            }
                        }
                    }
                }
            }
            finally
            {
                this.RedrawBufferedGraphic();
            }

        }
        #endregion

        private void Drow_Bg(Graphics gr)
        {
            Image image = Image.FromFile(@"C:\Users\user\source\repos\Ai_Integration_Plugin\Resources\icon_png.png");
            //Crea una ColorMatrix e applica il colore della maschera
            float r = 27 / 255f;
            float g = 43 / 255f;
            float b = 50 / 255f;

            // Calcola il nuovo rapporto di aspetto per adattarsi a Unit_Size mantenendo il rapporto di aspetto originale
            float scaleFactor = Math.Min((float)Unit_Size.Width / image.Width, (float)Unit_Size.Height / image.Height);
            int scaledWidth = (int)(image.Width * scaleFactor);
            int scaledHeight = (int)(image.Height * scaleFactor);

            // Crea il rettangolo centrato se necessario
            int offsetX = (Unit_Size.Width - scaledWidth) / 2;
            int offsetY = (Unit_Size.Height - scaledHeight) / 2;
            Rectangle destRect = new Rectangle(offsetX, offsetY, scaledWidth, scaledHeight);


            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {0, 0, 0, 0, 0},
                    new float[] {0, 0, 0, 0, 0},
                    new float[] {0, 0, 0, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {r, g, b, 0, 1}
                });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            //Disegna l'immagine originale con la matrice colore applicata
            //gr.DrawImage(image, new Rectangle(0, 0, this.Unit_Size.Width, this.Unit_Size.Height), 0, 0, Unit_Size.Width, Unit_Size.Height, GraphicsUnit.Pixel, attributes);
            gr.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

            //TODO: collegare questo argb ai temi
            Color maskColor = Color.FromArgb(250, 27, 43, 50);

            if (destRect.Width < Unit_Size.Width)
            {
                int delta = Unit_Size.Width - destRect.Width;

                Rectangle sx_rect = new Rectangle(0, 0, delta / 2 + 10, Unit_Size.Height);
                gr.FillRectangle(new SolidBrush(maskColor), sx_rect);

                Rectangle dx_rect = new Rectangle((this.Unit_Size.Width - (delta / 2) - 10), 0, delta / 2 + 11, Unit_Size.Height);
                gr.FillRectangle(new SolidBrush(maskColor), dx_rect);
            }

            if (destRect.Height < Unit_Size.Height)
            {
                int delta = Unit_Size.Height - destRect.Height;

                Rectangle top_rect = new Rectangle(0, 0, Unit_Size.Width, delta / 2 + 10);
                gr.FillRectangle(new SolidBrush(maskColor), top_rect);

                Rectangle btm_rect = new Rectangle(0, (this.Unit_Size.Height - (delta / 2) - 10), Unit_Size.Height, delta / 2 + 11);
                gr.FillRectangle(new SolidBrush(maskColor), btm_rect);
            }
        }
    }
}
