using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class CreateCustomerRequestDto
    {
        #region Ctor

        public CreateCustomerRequestDto()
        {
            LastName = "ناشناس";
            GenderId = 3;
            RelatedCompanies = new List<CustomerRelatedCompanyDto>();
            Emails = new List<string>();
            Mobiles = new List<string>();
            Phones = new List<string>();
            Webs = new List<string>();
            Faxes = new List<string>();
            Addresses = new List<CustomerAddressDto>();
            CompanyPartners = new List<int>();
            PersonPartners = new List<int>();
            IsMinData = false;
            PersonCustomFieldDtos = new List<CustomerCustomFieldsDto>();
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public string LastName { get; set; }

        public int GenderId { get; set; }

        public List<CustomerRelatedCompanyDto> RelatedCompanies { get; set; }

        public List<string> Emails { get; set; }

        public List<string> Mobiles { get; set; }

        public List<string> Phones { get; set; }

        public List<string> Webs { get; set; }

        public List<string> Faxes { get; set; }

        public List<CustomerAddressDto> Addresses { get; set; }

        public List<int> CompanyPartners { get; set; }

        public List<int> PersonPartners { get; set; }

        public int? AcquaintionTypeId { get; set; }

        public int OwnerUserId { get; set; }

        public string NationalCode { get; set; }

        public bool IsMinData { get; set; }

        public List<CustomerCustomFieldsDto> PersonCustomFieldDtos { get; set; }

        #endregion
    }
}
