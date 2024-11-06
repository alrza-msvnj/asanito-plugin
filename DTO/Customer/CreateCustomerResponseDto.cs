using System;
using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class CreateCustomerResponseDto
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string JobPosition { get; set; }

        public List<CustomerRelatedCompanyDto> RelatedCompanies { get; set; }

        public int GenderId { get; set; }

        public List<CustomerSocialDto> Socials { get; set; }

        public List<CustomerPhoneNumberDto> PhoneNumbers { get; set; }

        public DateTime? LastCallDate { get; set; }

        public TimeSpan? LastCallTime { get; set; }

        public List<CustomerAddressDto> Addresses { get; set; }

        public List<int> Partners { get; set; }

        public string ProfileImageUrl { get; set; }

        public int? AcquaintionTypeID { get; set; }

        public string GenderTitle { get; set; }

        public int? NegotiationID { get; set; }

        public int? OldSystemID { get; set; }

        public int Code { get; set; }

        public int OwnerUserID { get; set; }

        public string NationalCode { get; set; }

        public string ContactSupportLevel { get; set; }

        public string SupportCode { get; set; }

        public CustomerContactLabelDto ContactLabel { get; set; }

        public List<CustomerCustomFieldsDto> CustomFields { get; set; }

        public int Id { get; set; }

        public bool Deleted { get; set; }

        public string Description { get; set; }
    }
}