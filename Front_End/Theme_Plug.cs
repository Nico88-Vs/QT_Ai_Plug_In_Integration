using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Front_End
{
    public static class Theme_Plug
    {
        public static string LOGO_PATH = @"C:\Users\user\source\repos\Ai_Integration_Plugin\Resources\buona_buona_2.png";
        public static Color BackgroundColor { get; set; } = Color.FromArgb(255,27,43,50);
        public static Color ForegroundColor { get; set; } = Color.FromArgb(255,157, 73, 45);
        public static Font Font { get; set; } = new Font("Arial", 12);
        public static Pen BorderPen { get; set; } = new Pen(Color.FromArgb(255,41,144,128));
        public static Brush ForegroundBrush { get; set; } = Brushes.Black;
        public static Color Button_Background_color { get; set; } = Color.FromArgb(0, 27, 43, 50);
        public static SolidBrush Text_brush { get; set; } = new SolidBrush(Color.FromArgb(255, 157, 73, 45));
        public static SolidBrush Brash_Backcground_Button { get; set; } = new SolidBrush(Button_Background_color);
        public static int Fixed_Height { get; set; } = 30;
        public static int Row_span { get; } = 5;
        public static int Fixed_Pos_by_row_ind(int rw_index) => ((Fixed_Height/2)+Row_span)+(rw_index*(Fixed_Height)+Row_span);
    }
}
    