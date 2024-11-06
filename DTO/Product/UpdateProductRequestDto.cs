namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product
{
    public class UpdateProductRequestDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public int Type { get; set; }

        public int InitialBuyPrice { get; set; }

        public int UnitId { get; set; }

        public int EndPrice { get; set; }

        public int SellPrice { get; set; }
    }
}
