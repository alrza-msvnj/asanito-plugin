using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class UpdateInvoiceStatusRequestDto
    {
        #region Ctor

        public UpdateInvoiceStatusRequestDto()
        {
            InvoiceIds = new List<int>();
        }

        #endregion

        #region Properties

        public List<int> InvoiceIds { get; set; }

        public int Status { get; set; }

        public string SellDate { get; set; }

        #endregion
    }
}