using Nop.Core;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Domain
{
    public class AsanitoProductMapping : BaseEntity
    {
        #region

        public AsanitoProductMapping()
        {
            ProductAttributeCombinationId = 0;
        }

        #endregion

        #region Properties

        public int AsanitoProductId { get; set; }

        public int ProductId { get; set; }

        public int ProductAttributeCombinationId { get; set; }

        public int AsanitoCateogryId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public int StockQuantity { get; set; }

        public int InitialBuyPrice { get; set; }

        public int SellPrice { get; set; }

        public int AsanitoProductTypeId { get; set; }

        public int AsanitoUnitId { get; set; }

        public string RawResponse { get; set; }

        #endregion
    }
}