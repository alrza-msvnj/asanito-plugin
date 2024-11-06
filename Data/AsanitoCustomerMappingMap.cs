using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Data
{
    /// <summary>
    /// Represents Asanito Mapping Cofigure
    /// </summary>
    public class AsanitoCustomerMappingMap : NopEntityTypeConfiguration<AsanitoCustomerMapping>
    {
        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<AsanitoCustomerMapping> builder)
        {
            builder.ToTable(nameof(AsanitoCustomerMapping));
            builder.HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}