namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class CreateCustomerAddressResponseDto
    {
        public int? CityId { get; set; }

        public int ProvinceId { get; set; }

        public string Address { get; set; }

        public bool IsDefault { get; set; }

        public string PostalCode { get; set; }

        public int Id { get; set; }

        public bool Deleted { get; set; }

        public string Description { get; set; }
    }
}
