namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class CreateInvoiceResponseDto
    {
        #region Ctor

        public CreateInvoiceResponseDto()
        {
            Number = new InvoiceNumberDto();
            Contact = new InvoiceContactDto();
            Status = new InvoiceStatusDto();
            Type = new InvoiceTypeDto();
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Title { get; set; }

        public InvoiceNumberDto Number { get; set; }

        public string Organization { get; set; }

        public InvoiceContactDto Contact { get; set; }

        public string Date { get; set; }

        public string DueDate { get; set; }

        public int TotalDiscountedAmount { get; set; }

        public int RemainedAmount { get; set; }

        public InvoiceStatusDto Status { get; set; }

        public bool Editable { get; set; }

        public InvoiceTypeDto Type { get; set; }

        public string SellDate { get; set; }

        public int OwnerUserId { get; set; }

        public int TotalTax { get; set; }

        #endregion
    }
}