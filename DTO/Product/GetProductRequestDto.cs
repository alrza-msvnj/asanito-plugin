namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product
{
    public class GetProductRequestDto
    {
        #region Ctor

        public GetProductRequestDto()
        {
            Skip = 0;
            SearchValue = string.Empty;
            SortProp = string.Empty;
            OrderType = true;
        }

        #endregion

        #region Properties

        public int Skip { get; set; }

        public int Take { get; set; }

        public string SearchValue { get; set; }

        public string SortProp { get; set; }

        public bool OrderType { get; set; }

        #endregion
    }
}