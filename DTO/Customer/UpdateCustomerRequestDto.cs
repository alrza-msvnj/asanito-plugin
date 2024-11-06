namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class UpdateCustomerRequestDto
    {
        #region Ctor

        public UpdateCustomerRequestDto()
        {
            GenderId = 3;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public int GenderId { get; set; }

        public int? AcquaintionTypeId { get; set; }

        public int OwnerUserId { get; set; }

        public int? ContactLabelPriority { get; set; }

        public string NationalCode { get; set; }

        #endregion
    }
}