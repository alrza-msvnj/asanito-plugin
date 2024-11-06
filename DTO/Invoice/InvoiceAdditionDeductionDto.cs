using Newtonsoft.Json;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class InvoiceAdditionDeductionDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("calcType")]
        public bool CalcType { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}