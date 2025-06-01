using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DataAccess.Concrate.EfMapping
{
    public class EfUnitOfMeasureMapping : EfBaseEntityMapping<UnitOfMeasure>
    {
        public override void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(u => u.Id);

            // Property configurations
            builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Abbreviation).IsRequired().HasMaxLength(10);

            // Relationships
            builder.HasMany(u => u.Products)
                  .WithOne(p => p.UnitOfMeasure)
                  .HasForeignKey(p => p.UnitOfMeasureId);

            // Seed data
            var defaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData(
                new UnitOfMeasure
                {
                    Id = 1,
                    Name = "Adet",
                    Abbreviation = "AD",
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new UnitOfMeasure
                {
                    Id = 2,
                    Name = "Kilogram",
                    Abbreviation = "KG",
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new UnitOfMeasure
                {
                    Id = 3,
                    Name = "Litre",
                    Abbreviation = "LT",
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new UnitOfMeasure
                {
                    Id = 4,
                    Name = "Metre",
                    Abbreviation = "MT",
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new UnitOfMeasure
                {
                    Id = 5,
                    Name = "Paket",
                    Abbreviation = "PK",
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new UnitOfMeasure
                {
                    Id = 6,
                    Name = "Kutu",
                    Abbreviation = "KT",
                    Created = defaultDate,
                    Modified = defaultDate
                }
            );
        }
    }
}
