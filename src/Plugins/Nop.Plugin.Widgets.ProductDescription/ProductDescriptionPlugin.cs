using System.Collections.Generic;
using System.IO;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.ProductDescription
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class DescriptionPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public DescriptionPlugin(IPictureService pictureService,
            ISettingService settingService, IWebHelper webHelper)
        {
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> { "productdetails_overview_bottom" };
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsProductDescription/Configure";
        }

        /// <summary>
        /// Gets a view component for displaying plugin in public store
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <param name="viewComponentName">View component name</param>
        public void GetPublicViewComponent(string widgetZone, out string viewComponentName)
        {
            viewComponentName = "WidgetsProductDescription";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.ProductDescription.Text", "Description");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.ProductDescription.Description", "Product Description");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<ProductDescriptionSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.ProductDescription.Text");
            this.DeletePluginLocaleResource("Plugins.Widgets.ProductDescription.Description");
           
            base.Uninstall();
        }
    }
}