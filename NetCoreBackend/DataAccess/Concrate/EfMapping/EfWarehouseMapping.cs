using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfWarehouseMapping : EfBaseEntityMapping<Warehouse>
    {
        public override void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(w => w.Id);

            // Property configurations
            builder.Property(w => w.Name).IsRequired().HasMaxLength(100);

            // Relationships
            builder.HasOne(w => w.Company)
                  .WithMany()
                  .HasForeignKey(w => w.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Collections
            builder.HasMany(w => w.AddressWarehouses)
                  .WithOne(aw => aw.Warehouse)
                  .HasForeignKey(aw => aw.WarehouseId);

            builder.HasMany(w => w.ContactWarehouses)
                  .WithOne(cw => cw.Warehouse)
                  .HasForeignKey(cw => cw.WarehouseId);
        }
    }
}
