namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Warehouse
{
    public class GetWarehouseResponseDto
    {
        public string Title { get; set; }

        public string Code { get; set; }

        public int StationId { get; set; }

        public string Station { get; set; }

        public int? InChargeUserId { get; set; }

        public string InChargeUser { get; set; }

        public int Area { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public decimal StockCnt { get; set; }

        public int Id { get; set; }

        public bool Deleted { get; set; }

        public string Description { get; set; }
    }
}