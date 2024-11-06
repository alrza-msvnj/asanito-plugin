namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class CreateCustomerAddressRequestDto
    {
        #region Ctor

        public CreateCustomerAddressRequestDto()
        {
            IsDefault = true;
        }

        #endregion

        #region Properties

        public int ContactId { get; set; }

        public int CityId { get; set; }

        public string Address { get; set; }

        public bool IsDefault { get; set; }

        #endregion
    }
}
