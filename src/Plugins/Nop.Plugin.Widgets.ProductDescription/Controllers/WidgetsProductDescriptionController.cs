using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.ProductDescription.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.ProductDescription.Controllers
{
    [Area(AreaNames.Admin)]
    public class WidgetsProductDescriptionController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        public WidgetsProductDescriptionController(IWorkContext workContext,
            IStoreService storeService,
            IPermissionService permissionService, 
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._permissionService = permissionService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._localizationService = localizationService;
        }

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var ProductDescriptionSettings = _settingService.LoadSetting<ProductDescriptionSettings>(storeScope);
            var model = new ConfigurationModel
            {
                Description = ProductDescriptionSettings.Description,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.Description_OverrideForStore = _settingService.SettingExists(ProductDescriptionSettings, x => x.Description, storeScope);
            }

            return View("~/Plugins/Widgets.ProductDescription/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var productDescriptionSettings = _settingService.LoadSetting<ProductDescriptionSettings>(storeScope);



            productDescriptionSettings.Description = model.Description;
            

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(productDescriptionSettings, x => x.Description, model.Description_OverrideForStore, storeScope, false);
                      
            //now clear settings cache
            _settingService.ClearCache();
            
            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
    }
}