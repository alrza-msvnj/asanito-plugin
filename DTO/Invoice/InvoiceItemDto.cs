using Newtonsoft.Json;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class InvoiceItemDto
    {
        [JsonProperty("productID")]
        public int ProductId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("unitPrice")]
        public int UnitPrice { get; set; }

        [JsonProperty("discountType")]
        public bool DiscountType { get; set; }

        [JsonProperty("discountPercent")]
        public int DiscountPercent { get; set; }

        [JsonProperty("discountAmount")]
        public int DiscountAmount { get; set; }

        [JsonProperty("totalDiscountAmount")]
        public int TotalDiscountAmount { get; set; }

        [JsonProperty("productUnitID")]
        public int ProductUnitId { get; set; }

        [JsonProperty("hostWarehouseID")]
        public int HostWarehouseId { get; set; }

        [JsonProperty("productType")]
        public int ProductType { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }
    }
}
