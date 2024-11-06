using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class CreateSettledInvoiceRequestDto
    {
        #region Ctor

        public CreateSettledInvoiceRequestDto()
        {
            NegotiationId = 0;
            InvoiceType = 1;
            PersonIds = new List<int>();
            CompanyIds = new List<int>();
            Items = new List<InvoiceItemDto>();
            AdditionDeductions = new List<InvoiceAdditionDeductionDto>();
            OnlinePayment = false;
            PaymentType = true;
            CheckPayments = new List<InvoiceCheckPaymentDto>();
            CashPayments = new List<InvoiceCashPaymentDto>();
        }

        #endregion

        #region Properties

        public string Title { get; set; }

        public int NegotiationId { get; set; }

        public int OrganizationId { get; set; }

        public int InvoiceType { get; set; }

        public string Date { get; set; }

        public int PersonId { get; set; }

        public List<int> PersonIds { get; set; }

        public List<int> CompanyIds { get; set; }

        public List<InvoiceItemDto> Items { get; set; }

        public List<InvoiceAdditionDeductionDto> AdditionDeductions { get; set; }

        public bool OnlinePayment { get; set; }

        public int BankAccountId { get; set; }

        public int OwnerUserId { get; set; }

        public bool PaymentType { get; set; }

        public List<InvoiceCheckPaymentDto> CheckPayments { get; set; }

        public List<InvoiceCashPaymentDto> CashPayments { get; set; }

        public string SellDate { get; set; }

        #endregion
    }
}