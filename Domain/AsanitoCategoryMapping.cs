using Nop.Core;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Domain
{
    public class AsanitoCategoryMapping : BaseEntity
    {
        public int AsanitoCategoryId { get; set; }

        public int CategoryId { get; set; }

        public string RawResponse { get; set; }
    }
}