using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfAddressMapping : EfBaseEntityMapping<Address>
    {
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(a => a.Id);

            // Property configurations
            builder.Property(a => a.Location).HasMaxLength(100);
            builder.Property(a => a.Type).HasMaxLength(50);
            builder.Property(a => a.Street).HasMaxLength(200);
            builder.Property(a => a.City).HasMaxLength(50);
            builder.Property(a => a.State).HasMaxLength(50);
            builder.Property(a => a.Country).HasMaxLength(50);
            builder.Property(a => a.ZipCode).HasMaxLength(20);

            // Relationships
            builder.HasMany(a => a.AddressWarehouses)
                  .WithOne(aw => aw.Address)
                  .HasForeignKey(aw => aw.AddressId);

            builder.HasMany(a => a.AddressPartners)
                  .WithOne(ap => ap.Address)
                  .HasForeignKey(ap => ap.AddressId);
        }
    }
}
