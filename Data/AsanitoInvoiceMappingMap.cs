using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Data
{
    /// <summary>
    /// Represents Asanito Mapping Cofigure
    /// </summary>
    public class AsanitoInvoiceMappingMap : NopEntityTypeConfiguration<AsanitoInvoiceMapping>
    {
        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<AsanitoInvoiceMapping> builder)
        {
            builder.ToTable(nameof(AsanitoInvoiceMapping));
            builder.HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}