namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Warehouse
{
    public class GetWarehouseStockResponseDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }

        public GetWarehouseResponseDto Warehouse { get; set; }

        public int WarehouseId { get; set; }

        public decimal InitialStockCnt { get; set; }

        public decimal CurrentStockCnt { get; set; }
    }
}