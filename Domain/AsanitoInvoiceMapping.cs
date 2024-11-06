using Nop.Core;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Domain
{
    public class AsanitoInvoiceMapping : BaseEntity
    {
        public int AsanitoInvoiceId { get; set; }

        public int OrderId { get; set; }

        public string RawResponse { get; set; }
    }
}