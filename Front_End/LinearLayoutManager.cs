using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TradingPlatform.BusinessLayer;
using Front_End.Renderables;
using Ai_Integration_Plugin_due;
using Services;
using Services.Command;
using System.Threading;
using Models;


namespace Front_End
{
    public class LinearLayoutManager
    {
        private static LinearLayoutManager instance;
        private static readonly object lockObject = new object();
        private Indicator _indicator;
        private Symbol _symbol;
        private AiSetter _aiSetter;

        public Indicator Indicator 
        { get { return this._indicator; } 
            set 
            {
                if (value != this._indicator)
                {
                    this._indicator = value;
                }

            }
        }
        public Symbol Current_Symbol
        {
            get { return this._symbol; }
            set
            {
                if (value != this._symbol)
                {
                    this._symbol = value;
                }
            }
        }
        public static LinearLayoutManager Instance
        {
            get
            {
                lock (lockObject) { if (instance == null) { instance = new LinearLayoutManager(); } return instance; }
            }
        }
        public List<IRenderable> controls { get; set; } = new List<IRenderable>();
        public List<Column> Columns { get; set; } = new List<Column>();
        private Size _size;
        public Pages _page = Pages.Run;
        public Size Size{ get => this._size; }
        private List<string> _str = new List<string>() { "uno", "due", "tre", "quattro" };
        public event EventHandler<Pages> PageChanged;

        private LinearLayoutManager()
        {
        }

        public virtual void OnPageChanged(Pages page)
        {
            this.PageChanged?.Invoke(Instance, page);
        }

        public void Inizialize(Symbol symbol, Indicator indicator, Size size)
        {
            this._size = size;
            this.Current_Symbol = symbol;
            this.Indicator = indicator;
            this._aiSetter = new AiSetter();
            this.Resize(this.Size);
        }

        public void Switch_Page(Pages page)
        {
            this._page = page;
            this.controls.Clear();

            switch (page)
            {
                case Pages.Run:
                 
                    ButtonRenderable Lunch_btn = new ButtonRenderable(UI_usage.Lunch_Python, new Rectangle((this.Size.Width / 2) - (int)(this.Columns[1].CurrentWidth * 1.5), (this.Size.Height / 2) - Theme_Plug.Fixed_Height, this.Columns[1].CurrentWidth * 3, Theme_Plug.Fixed_Height * 2));
                    controls.Add(Lunch_btn);

                    Text_Renderable Base_Path = new Text_Renderable(new Rectangle((this.Size.Width / 2) - (int)(this.Columns[1].CurrentWidth * 1.5), (this.Size.Height / 3) - Theme_Plug.Fixed_Height, this.Columns[1].CurrentWidth * 3, Theme_Plug.Fixed_Height * 2), $"Base Log Path : {this._aiSetter._db_Reference.BasePath}");
                    controls.Add(Base_Path);

                    Text_Renderable ConnecString = new Text_Renderable(new Rectangle((this.Size.Width / 2) - (int)(this.Columns[1].CurrentWidth * 1.5), (this.Size.Height / 4) - Theme_Plug.Fixed_Height, this.Columns[1].CurrentWidth * 3, Theme_Plug.Fixed_Height * 2), $"Connection String : {this._aiSetter._db_Reference.Logs_Db}");
                    controls.Add(ConnecString);

                    DropdownMenuRenderable dd1 = new DropdownMenuRenderable(UI_usage.Drop_Down, new Rectangle(this.Columns[9].Col_Center, Theme_Plug.Fixed_Pos_by_row_ind(1), this.Columns[1].CurrentWidth * 2, Theme_Plug.Fixed_Height));
                    dd1.Items.Add("quattro");
                    dd1.Items.Add("quattro");
                    dd1.Items.Add("quattro");
                    dd1.Items.Add("quattro");
                    dd1.Items.Add("quattro");
                    controls.Add(dd1);


                    break;

                case Pages.Started:
                    Text_Renderable Model_Name = new Text_Renderable(new Rectangle(this.Columns[5].Col_Center, Theme_Plug.Fixed_Pos_by_row_ind(1), this.Columns[5].CurrentWidth * 3, Theme_Plug.Fixed_Height), "Select Your Model :");
                    controls.Add(Model_Name);

                    Text_Renderable Indis_name = new Text_Renderable(new Rectangle(this.Columns[5].Col_Center, Theme_Plug.Fixed_Pos_by_row_ind(2), this.Columns[5].CurrentWidth * 3, Theme_Plug.Fixed_Height), $"Indicator : {this.Indicator.ShortName}");
                    controls.Add(Indis_name);

                    Text_Renderable Pair_name = new Text_Renderable(new Rectangle(this.Columns[5].Col_Center, Theme_Plug.Fixed_Pos_by_row_ind(3), this.Columns[5].CurrentWidth * 3, Theme_Plug.Fixed_Height), $"Pair : {this.Current_Symbol?.Name ?? "N/A"}");
                    controls.Add(Pair_name);

                    Images_Renderable logo = new Images_Renderable(Theme_Plug.LOGO_PATH, new Rectangle(this.Columns[1].Col_Center - (this.Columns[1].CurrentWidth / 2), Theme_Plug.Fixed_Pos_by_row_ind(1) - (Theme_Plug.Fixed_Height / 2), this.Columns[8].CurrentWidth * 2, Theme_Plug.Fixed_Height * 2), UI_usage.Logo);
                    controls.Add(logo);

                    ButtonRenderable Run_Btn = new ButtonRenderable(UI_usage.Back_ToStart, new Rectangle(this.Columns[11].Col_Center, Theme_Plug.Fixed_Pos_by_row_ind(11), this.Columns[11].CurrentWidth, Theme_Plug.Fixed_Height));
                    controls.Add(Run_Btn);

                    DropdownMenuRenderable dd = new DropdownMenuRenderable(UI_usage.Drop_Down, new Rectangle(this.Columns[9].Col_Center, Theme_Plug.Fixed_Pos_by_row_ind(1), this.Columns[1].CurrentWidth * 2, Theme_Plug.Fixed_Height));
                    dd.Items.Add("quattro");
                    dd.Items.Add("quattro");
                    dd.Items.Add("quattro");
                    dd.Items.Add("quattro");
                    dd.Items.Add("quattro");
                    controls.Add(dd);

                    var lista = this.Build_Items(this._str, 6);

                    for (int i = 0; i < lista.Count; i++)
                    {
                        this.controls.Add(lista[i]);
                    }

                    break;
            }

            foreach (var item in controls)
            {
                item.Clicked += this.Item_Clicked;
            }
        }

        private void Item_Clicked(object sender, EventArgsCommand e)
        {
            if (e.Command.GetType() == typeof(ChangePageCommand))
            {
                var x = (ChangePageCommand)e.Command;
                this.OnPageChanged(x.DestinatioPage);
            }
        }

        public void Resize(Size size)
        {
            this._size = size;

            this.Columns.Clear();
            for (int i = 0; i < 12; i++)
            {
                Column col = new Column(size.Width / 12, i);
                this.Columns.Add(col);
            }

            this.Switch_Page(this._page);
        }

        private List<IRenderable> Build_Items(List<string> items_string, int starting_Row)
        {
            int co_index_txt = this.Columns[1].Col_Center;
            int co_index_dd = this.Columns[5].Col_Center;
            int rw_ind = starting_Row;
            int w = this.Columns[1].CurrentWidth * 3;
            int h = Theme_Plug.Fixed_Height;

            List<IRenderable> renderables = new List<IRenderable>();
            for (int i = 0; i < items_string.Count; i++)
            {
                Text_Renderable str = new Text_Renderable(new Rectangle(co_index_txt, Theme_Plug.Fixed_Pos_by_row_ind(rw_ind), w, h), items_string[i]);
                renderables.Add(str);

                DropdownMenuRenderable dd = new DropdownMenuRenderable(UI_usage.Drop_Down, new Rectangle(co_index_dd,Theme_Plug.Fixed_Pos_by_row_ind(rw_ind), w, h));
                renderables.Add(dd);

                rw_ind++;
            }

            //TODO: attenzione a questa possibile eccezione
            return renderables;

        }
    }
}
