namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class InvoiceCashPaymentDto
    {
        #region Ctor

        public InvoiceCashPaymentDto()
        {
            Description = "";
            Amount = 0;
            AmountInWord = "";
        }

        #endregion

        #region Properties

        public string Description { get; set; }

        public string Date { get; set; }

        public int Amount { get; set; }

        public int AccountId { get; set; }

        public string AmountInWord { get; set; }

        #endregion
    }
}