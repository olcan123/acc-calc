using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfProductPriceMapping : EfBaseEntityMapping<ProductPrice>
    {
        public override void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(pp => pp.Id);

            // Property configurations
            builder.Property(pp => pp.UnitPrice).HasColumnType("decimal(18,6)").IsRequired();
            builder.Property(pp => pp.Category).HasConversion<byte>();
            builder.Property(pp => pp.Side).HasConversion<byte>();
            builder.Property(pp => pp.ValidFrom).IsRequired(false);
            builder.Property(pp => pp.ValidTo).IsRequired(false);
    
            
            // Relationships
            builder.HasOne(pp => pp.Product)
                  .WithMany(p => p.ProductPrices)
                  .HasForeignKey(pp => pp.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
