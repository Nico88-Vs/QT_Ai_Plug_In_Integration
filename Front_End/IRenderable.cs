using System;
using System.Diagnostics;
using System.Drawing;
using Services;
using Services.Command;
using TradingPlatform.BusinessLayer.Integration;

namespace Front_End
{
    public interface IRenderable
    {
        void Draw(Graphics graphics);
        Rectangle Bounds { get; set; }
        UI_usage B_Usage { get; set; }
        int Min_Widt { get; }
        int Max_Widt { get; }
        void SetBounds(Rectangle bounds);
        Size GetPreferredSize(Graphics graphics);

        event EventHandler<EventArgsCommand> Clicked;

        virtual void OnClick()
        {
            
        }
    }
}
