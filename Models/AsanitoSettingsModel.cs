using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Models
{
    public class AsanitoSettingsModel : BaseNopModel, ISettingsModel
    {
        #region Ctor

        public AsanitoSettingsModel()
        {
            AvailableCustomerCreationTypes = new List<SelectListItem>();
            AvailableInvoiceCreationTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.Username")]
        public string Username { get; set; }

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.Password")]
        public string Password { get; set; }

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.CustomerCreationTypeId")]
        public int CustomerCreationTypeId { get; set; }

        public CustomerCreationType CustomerCreationType
        {
            get => (CustomerCreationType)CustomerCreationTypeId;
            set => CustomerCreationTypeId = (int)value;
        }

        public IList<SelectListItem> AvailableCustomerCreationTypes { get; set; }

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.InvoiceCreationTypeId")]
        public int InvoiceCreationTypeId { get; set; }

        public InvoiceCreationType InvoiceCreationType
        {
            get => (InvoiceCreationType)InvoiceCreationTypeId;
            set => InvoiceCreationTypeId = (int)value;
        }

        public IList<SelectListItem> AvailableInvoiceCreationTypes { get; set; }

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.OrganizationId")]
        public int OrganizationId { get; set; }

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.BankAccountId")]
        public int BankAccountId { get; set; }

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.OwnerUserId")]
        public int OwnerUserId { get; set; }

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.ProductUnitId")]
        public int ProductUnitId { get; set; }

        [NopResourceDisplayName("Tesla.Plugin.Widgets.CRM.Asanito.AsanitoSettingsModel.WarehouseId")]
        public int WarehouseId { get; set; }

        public int ActiveStoreScopeConfiguration { get; set; }

        #endregion
    }
}