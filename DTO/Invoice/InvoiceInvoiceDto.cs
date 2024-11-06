namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class InvoiceInvoiceDto
    {
        #region Ctor

        public InvoiceInvoiceDto()
        {
            Number = "0";
        }

        #endregion

        #region Properties

        public int Amount { get; set; }

        public string Number { get; set; }

        public int InvoiceId { get; set; }

        #endregion
    }
}