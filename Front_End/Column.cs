using System;
using System.Collections.Generic;
using System.Linq;
using Front_End;

namespace Ai_Integration_Plugin
{
    public class Column
    {
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int CurrentWidth { get; set; }
        public List<IRenderable> Items { get; private set; }
        public int Col_Index { get; set; }
        public int Col_Center => (this.CurrentWidth / 2) + (this.Col_Index * this.CurrentWidth);

        public Column(int width, int col_Index)
        {
            this.CurrentWidth = width;
            this.Col_Index = col_Index;
        }

        public void AddItem(IRenderable item)
        {
            Items.Add(item);
        }

        public void Update_Width(int new_widt)
        {
            int min_w = this.Items.OrderByDescending(x => x.Min_Widt).First().Min_Widt;
            if (new_widt > min_w)
            {
                this.CurrentWidth = new_widt;
            }
        }
    }

}
