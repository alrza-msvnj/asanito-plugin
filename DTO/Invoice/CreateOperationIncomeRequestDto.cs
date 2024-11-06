using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class CreateOperationIncomeRequestDto
    {
        #region Ctor

        public CreateOperationIncomeRequestDto()
        {
            CompanyId = 0;
            Description = "";
            PaymentType = true;
            WalletCharge = false;
            Invoices = new List<InvoiceInvoiceDto>();
            CashPayments = new List<InvoiceCashPaymentDto>();
            CheckPayments = new List<InvoiceCheckPaymentDto>();
        }

        #endregion

        #region Properties

        public int PersonId { get; set; }

        public int CompanyId { get; set; }

        public string Description { get; set; }

        public bool PaymentType { get; set; }

        public bool WalletCharge { get; set; }

        public List<InvoiceInvoiceDto> Invoices { get; set; }

        public List<InvoiceCashPaymentDto> CashPayments { get; set; }

        public List<InvoiceCheckPaymentDto> CheckPayments { get; set; }

        #endregion
    }
}