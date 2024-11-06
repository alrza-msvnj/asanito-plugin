using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Data
{
    public class AsanitoCategoryMappingMap : NopEntityTypeConfiguration<AsanitoCategoryMapping>
    {
        public override void Configure(EntityTypeBuilder<AsanitoCategoryMapping> builder)
        {
            builder.ToTable(nameof(AsanitoCategoryMapping));
            builder.HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}