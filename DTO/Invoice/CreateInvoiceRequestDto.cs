using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class CreateInvoiceRequestDto
    {
        #region Ctor

        public CreateInvoiceRequestDto()
        {
            NegotiationId = 0;
            InvoiceType = 1;
            PersonIds = new List<int>();
            CompanyIds = new List<int>();
            Items = new List<InvoiceItemDto>();
            AdditionDeductions = new List<InvoiceAdditionDeductionDto>();
            OnlinePayment = false;
        }

        #endregion

        #region Properties

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("negotiationID")]
        public int NegotiationId { get; set; }

        [JsonProperty("organizationID")]
        public int OrganizationId { get; set; }

        [JsonProperty("invoiceType")]
        public int InvoiceType { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("personID")]
        public int PersonId { get; set; }

        [JsonProperty("personIDs")]
        public List<int> PersonIds { get; set; }

        [JsonProperty("companyIDs")]
        public List<int> CompanyIds { get; set; }

        [JsonProperty("items")]
        public List<InvoiceItemDto> Items { get; set; }

        [JsonProperty("additionDeductions")]
        public List<InvoiceAdditionDeductionDto> AdditionDeductions { get; set; }

        [JsonProperty("onlinePayment")]
        public bool OnlinePayment { get; set; }

        [JsonProperty("bankAccountID")]
        public int BankAccountId { get; set; }

        [JsonProperty("ownerUserID")]
        public int OwnerUserId { get; set; }

        #endregion
    }
}