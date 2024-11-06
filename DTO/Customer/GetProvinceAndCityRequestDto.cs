namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class GetProvinceAndCityRequestDto
    {
        #region Ctor

        public GetProvinceAndCityRequestDto()
        {
            Take = 35;
            Id = 0;
            Skip = 0;
        }

        #endregion

        #region Properties

        public int Take { get; set; }

        public int Id { get; set; }

        public int Skip { get; set; }

        public string Value { get; set; }

        #endregion
    }
}
