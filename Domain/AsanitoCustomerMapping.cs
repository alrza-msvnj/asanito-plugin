using Nop.Core;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Domain
{
    public class AsanitoCustomerMapping : BaseEntity
    {
        public int AsanitoCustomerId  { get; set; }

        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int GenderId { get; set; }

        public string RawResponse { get; set; }
    }
}