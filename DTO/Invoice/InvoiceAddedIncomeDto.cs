namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class InvoiceAddedIncomeDto
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public InvoiceContactDto Contact { get; set; }

        public bool PaymentType { get; set; }

        public int Amount { get; set; }

        public InvoiceAccountDto Account { get; set; }

        public string Date { get; set; }

        public int CheckStatus { get; set; }

        public string DueDate { get; set; }
    }
}