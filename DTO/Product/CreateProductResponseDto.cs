using System.Collections.Generic;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Category;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product
{
    public class CreateProductResponseDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }

        public string Title { get; set; }

        public decimal Stock { get; set; }

        public int SellPrice { get; set; }

        public int BuyPrice { get; set; }

        public ProductProductTypeDto ProductType { get; set; }

        public CategoryDto Category { get; set; }

        public int InitialBuyPrice { get; set; }

        public int UnitId { get; set; }

        public int EndPrice { get; set; }

        public List<string> Barcodes { get; set; }

        public int Code { get; set; }

        public bool HasSerialPerStock { get; set; }
    }
}