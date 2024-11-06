using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product
{
    public class CreateProductRequestDto
    {
        #region Ctor

        public CreateProductRequestDto()
        {
            Type = 1;
            UnitId = 0;
            WarehouseStock = new List<ProductWarehouseDto>();
        }

        #endregion

        #region Properties

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public int Type { get; set; }

        public int InitialBuyPrice { get; set; }

        public int UnitId { get; set; }

        public int EndPrice { get; set; }

        public int SellPrice { get; set; }

        public List<ProductWarehouseDto> WarehouseStock { get; set; }

        public bool HasSerialStock { get; set; }

        #endregion
    }
}