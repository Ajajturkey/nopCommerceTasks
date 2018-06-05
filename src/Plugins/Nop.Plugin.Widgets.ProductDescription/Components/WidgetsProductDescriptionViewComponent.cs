using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.ProductDescription.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.ProductDescription.Components
{
    [ViewComponent(Name = "WidgetsProductDescription")]
    public class WidgetsProductDescriptionViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;

        public WidgetsProductDescriptionViewComponent(IStoreContext storeContext, 
            IStaticCacheManager cacheManager, 
            ISettingService settingService, 
            IPictureService pictureService)
        {
            this._storeContext = storeContext;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var productDescription = _settingService.LoadSetting<ProductDescriptionSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel
            {
                Description = productDescription.Description
            };

            if (string.IsNullOrEmpty(model.Description))
                //no text found
                return Content("");

            return View("~/Plugins/Widgets.ProductDescription/Views/PublicInfo.cshtml", model);
        }

    }
}
