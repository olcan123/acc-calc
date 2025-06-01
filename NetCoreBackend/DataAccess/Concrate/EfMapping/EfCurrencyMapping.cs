using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DataAccess.Concrate.EfMapping
{
    public class EfCurrencyMapping : EfBaseEntityMapping<Currency>
    {
        public override void Configure(EntityTypeBuilder<Currency> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(c => c.Id);

            // Property configurations
            builder.Property(c => c.Code).IsRequired().HasMaxLength(10);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            
            // Relationships                  
            builder.HasMany(c => c.PurchaseInvoices)
                  .WithOne(pi => pi.Currency)
                  .HasForeignKey(pi => pi.CurrencyId);
                  
            builder.HasMany(c => c.BankAccounts)
                  .WithOne(ba => ba.Currency)
                  .HasForeignKey(ba => ba.CurrencyId);
                  
            // Seed data
            var defaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData(
                new Currency 
                { 
                    Id = 1, 
                    Code = "EUR", 
                    Name = "Euro", 
                    Created = defaultDate, 
                    Modified = defaultDate 
                },
                new Currency 
                { 
                    Id = 2, 
                    Code = "USD", 
                    Name = "US Dollar", 
                    Created = defaultDate, 
                    Modified = defaultDate 
                },
                new Currency 
                { 
                    Id = 3, 
                    Code = "ALL", 
                    Name = "Albanian Lek", 
                    Created = defaultDate, 
                    Modified = defaultDate 
                }
            );
        }
    }
}
