using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfVatMapping : EfBaseEntityMapping<Vat>
    {
        public override void Configure(EntityTypeBuilder<Vat> builder)
        {
            base.Configure(builder);

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Name).IsRequired().HasMaxLength(50);
            builder.Property(v => v.Rate).IsRequired();
            builder.Property(v => v.IsDefault).HasDefaultValue(false);
            builder.Property(v => v.IsActive).HasDefaultValue(true);

        // Seed data for VAT rates
            var defaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData(
                new Vat
                {
                    Id = 1,
                    Name = "TVSH %0",
                    Rate = 0,
                    IsDefault = false,
                    IsActive = true,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Vat
                {
                    Id = 2,
                    Name = "TVSH %8",
                    Rate = 8,
                    IsDefault = false,
                    IsActive = true,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Vat
                {
                    Id = 3,
                    Name = "TVSH %18",
                    Rate = 18,
                    IsDefault = true,
                    IsActive = true,
                    Created = defaultDate,
                    Modified = defaultDate
                }
            );
        }
    }
}
