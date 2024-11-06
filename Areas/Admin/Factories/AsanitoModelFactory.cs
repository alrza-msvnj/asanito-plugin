using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Tesla.Plugin.Widgets.CRM.Asanito.Client;
using Tesla.Plugin.Widgets.CRM.Asanito.Models;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Areas.Admin.Factories
{
    public class AsanitoModelFactory : IAsanitoModelFactory
    {
        #region Fields

        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IAsanitoClient _asanitoClient;

        #endregion

        #region Ctor

        public AsanitoModelFactory(IStoreContext storeContext, ISettingService settingService, ILocalizationService localizationService, IAsanitoClient asanitoClient)
        {
            _storeContext = storeContext;
            _settingService = settingService;
            _localizationService = localizationService;
            _asanitoClient = asanitoClient;
        }

        #endregion

        #region Methods

        public AsanitoSettingsModel PrepareAsanitoSettingsModel()
        {
            var storeId = _storeContext.CurrentStore.Id;
            var settings = _settingService.LoadSetting<AsanitoSettings>(storeId);
            var model = settings.ToSettingsModel<AsanitoSettingsModel>();

            model.AvailableCustomerCreationTypes.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.AvailableCustomerCreationTypes.Registration"),
                Value = "1",
            });
            model.AvailableCustomerCreationTypes.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.AvailableCustomerCreationTypes.PlaceOrder"),
                Value = "2",
            });

            model.AvailableInvoiceCreationTypes.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.AvailableInvoiceCreationTypes.OrderPlaced"),
                Value = "1",
            });
            model.AvailableInvoiceCreationTypes.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.AvailableInvoiceCreationTypes.OrderPaid"),
                Value = "2",
            });

            return model;
        }

        #endregion
    }
}