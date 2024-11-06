using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Data
{
    /// <summary>
    /// Represents Asanito Mapping Cofigure
    /// </summary>
    public class AsanitoProductMappingMap : NopEntityTypeConfiguration<AsanitoProductMapping>
    {
        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<AsanitoProductMapping> builder)
        {
            builder.ToTable(nameof(AsanitoProductMapping));
            builder.HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}