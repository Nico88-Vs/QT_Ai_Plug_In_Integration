using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TradingPlatform.BusinessLayer;
using TradingPlatform.PresentationLayer.Plugins;
using TradingPlatform.PresentationLayer.Renderers.Toolbar;

namespace Ai_Integration_Plugin
{
    public class Ai_Integration_Plugin : Plugin
    {
        #region GlobalVar
        private GdiRenderer gdiRenderer;
        private readonly SettingItemSeparatorGroup Base_set_group = new SettingItemSeparatorGroup("Selected Base settings", 0);
        private readonly SettingItemSeparatorGroup indicator_set_group = new SettingItemSeparatorGroup("Selected Indicator settings", 1);
        private string _current_Short = Core.Instance.Indicators.All[0].ShortName;
        private Symbol _symbol;
        private Indicator _current_indicator;
        #endregion

        /// <summary>
        /// Plugin meta information
        /// </summary>
        public static PluginInfo GetInfo()
        {
            var windowParameters = NativeWindowParameters.Panel;
            windowParameters.AllowDrop = true;
            windowParameters.BrowserUsageType = BrowserUsageType.None;

            return new PluginInfo()
            {
                Name = "Ai_Integration_Plugin",
                Title = "Ai_Integration_Plugin",
                Group = PluginGroup.Misc,
                ShortName = "AI_",
                SortIndex = 35,
                AllowSettings = true,
                WindowParameters = windowParameters,
                CustomProperties = new Dictionary<string, object>()
                {
                    {PluginInfo.Const.ALLOW_MANUAL_CREATION, true }
                }
            };
        }

        /// <summary>
        /// Default plugin size - can be an absolute value or a multiple of UnitSize (UnitSize depends on the monitor resolution)
        /// </summary>
        public override Size DefaultSize => new Size(this.UnitSize.Width * 1, this.UnitSize.Height * 2);

        /// <summary>
        /// Initialize called once on plugin creation
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            var info = Core.Instance.Indicators.All.First(x => x.Name == _current_Short);

            this._current_indicator = Core.Instance.Indicators.CreateIndicator(info);

            this._symbol = Core.Instance.Symbols[0];

            this.gdiRenderer = new GdiRenderer(this.Window.CreateRenderingControl("GdiRenderer"), this.UnitSize, this._symbol, this._current_indicator);
        }

        /// <summary>
        /// Populate called on plugin creation and each time when any connection get connected/disconnected
        /// </summary>
        public override void Populate(PluginParameters args = null)
        {
            base.Populate(args);

            this.gdiRenderer.RedrawBufferedGraphic();
        }

        public override void UpdateSettings()
        {
            this.gdiRenderer.Update_Lay(this._symbol, this._current_indicator);
        }

        public override void Dispose()
        {
            if (this.gdiRenderer != null)
            {
                this.gdiRenderer.Dispose();
                this.gdiRenderer = null;
            }

            base.Dispose();
        }

        public override IList<SettingItem> Settings
        {
            get
            {
                var result = base.Settings;


                if (this._symbol == null)
                    this._symbol = Core.Instance.Symbols[0];


                SettingItemSymbol s_item = new SettingItemSymbol("Symbol", Core.Instance.Symbols.First(x => x.Id == this._symbol.Id)) { SeparatorGroup = this.Base_set_group };

                List<string> indi_short_names = Core.Instance.Indicators.All.Select(info => info.ShortName).ToList();

                SettingItemSelector indi_item = new SettingItemSelector(name: "Indicator", items: indi_short_names, value: this._current_indicator.ShortName) { SeparatorGroup = this.Base_set_group };


                SettingItemAction action_defoult = new SettingItemAction("Save_ASDefoult", new SettingItemActionDelegate(Settings_As_Defoult));

                result.Add(s_item);
                result.Add(indi_item);
                result.Add(action_defoult);

                foreach (var item in this._current_indicator.Settings)
                {
                    item.SeparatorGroup = this.indicator_set_group;
                    result.Add(item);
                }


                return result;
            }
            set
            {
                base.Settings = value;

                if (value.GetItemByName("Symbol") is SettingItemSymbol symbol)
                {
                    this._symbol = Core.Instance.Symbols.FirstOrDefault(x => x == symbol.Value);
                }

                if (value.GetItemByName("Indicator") is SettingItemSelector indicator)
                {
                    var indi_info = Core.Instance.Indicators.All.First(x => x.ShortName == indicator);
                    var indi = Core.Instance.Indicators.CreateIndicator(indi_info);

                    this._current_indicator = indi;
                    this._current_Short = indi.ShortName;
                }

                this.UpdateSettings();
            }
        }

        public object Settings_As_Defoult(object args = null)
        {
            SaveCurrentSettingsAsDefault();
            return args;
        }


        protected override void OnLayoutUpdated()
        {
            base.OnLayoutUpdated();

            if (this.gdiRenderer != null)
            {
                this.gdiRenderer.RedrawBufferedGraphic();
                this.gdiRenderer.Layout.Margin = this.NonClientMargin;
            }
        }
    }
}
