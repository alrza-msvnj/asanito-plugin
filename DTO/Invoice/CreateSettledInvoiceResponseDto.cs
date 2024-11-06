namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class CreateSettledInvoiceResponseDto
    {
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

        public int OwnerUserID { get; set; }

        public int TotalTax { get; set; }
    }
}