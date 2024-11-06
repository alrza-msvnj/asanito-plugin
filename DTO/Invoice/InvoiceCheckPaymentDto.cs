namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class InvoiceCheckPaymentDto
    {
        public int Amount { get; set; }

        public string AmountInWord { get; set; }

        public string ReciptDate { get; set; }

        public string DueDate { get; set; }

        public int AccountId { get; set; }
    }
}