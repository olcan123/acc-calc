using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DataAccess.Concrate.EfMapping
{
    public class EfCompanyMapping : EfBaseEntityMapping<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(c => c.Id);

            // Property configurations
            builder.Property(c => c.Name).IsRequired().HasMaxLength(500);
            builder.Property(c => c.TradeName).HasMaxLength(500);
            builder.Property(c => c.UidNumber).HasMaxLength(50);
            builder.Property(c => c.VatNumber).HasMaxLength(50);
            builder.Property(c => c.Period).HasMaxLength(5);

            // Relationships
            builder.HasMany(c => c.BankAccountCompanies)
                  .WithOne(bac => bac.Company)
                  .HasForeignKey(bac => bac.CompanyId);

            builder.HasMany(c => c.Warehouses)
                  .WithOne(w => w.Company)
                  .HasForeignKey(w => w.CompanyId);
                
        }
    }
}
