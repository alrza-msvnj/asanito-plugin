using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Configuration;
using System.Collections.Generic;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;

namespace Tesla.Plugin.Widgets.CRM.Asanito
{
    public class AsanitoSettings : ISettings
    {
        #region Ctor

        public AsanitoSettings()
        {
            AvailableCustomerCreationTypes = new List<SelectListItem>();
            AvailableInvoiceCreationTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public string Username { get; set; }

        public string Password { get; set; }

        public int CustomerCreationTypeId { get; set; }

        public CustomerCreationType CustomerCreationType
        {
            get => (CustomerCreationType)CustomerCreationTypeId;
            set => CustomerCreationTypeId = (int)value;
        }

        public IList<SelectListItem> AvailableCustomerCreationTypes { get; set; }

        public int InvoiceCreationTypeId { get; set; }

        public InvoiceCreationType InvoiceCreationType
        {
            get => (InvoiceCreationType)InvoiceCreationTypeId;
            set => InvoiceCreationTypeId = (int)value;
        }

        public IList<SelectListItem> AvailableInvoiceCreationTypes { get; set; }

        public int OrganizationId { get; set; }

        public int ActiveStoreScopeConfiguration { get; set; }

        public int BankAccountId { get; set; }

        public int OwnerUserId { get; set; }

        public int ProductUnitId { get; set; }

        public int WarehouseId { get; set; }

        public Dictionary<string, object> CustomProperties { get; set; }

        #endregion
    }
}