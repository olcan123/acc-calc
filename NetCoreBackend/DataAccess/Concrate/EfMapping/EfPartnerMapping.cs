using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfPartnerMapping : EfBaseEntityMapping<Partner>
    {
        public override void Configure(EntityTypeBuilder<Partner> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(p => p.Id);

            // Property configurations
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.TradeName).HasMaxLength(100);
            builder.Property(p => p.PartnerType).HasConversion<int>();
            builder.Property(p => p.BusinessPartnerType).HasConversion<int?>();
            builder.Property(p => p.IdentityNumber).HasMaxLength(50);
            builder.Property(p => p.VatNumber).HasMaxLength(50);

            // Relationships
            builder.HasMany(p => p.PurchaseInvoices)
                  .WithOne(pi => pi.Partner)
                  .HasForeignKey(pi => pi.PartnerId);
                  
            builder.HasMany(p => p.PurchaseInvoiceExpenses)
                  .WithOne(pe => pe.Partner)
                  .HasForeignKey(pe => pe.PartnerId);
                  
            builder.HasMany(p => p.BankAccountPartners)
                  .WithOne(bap => bap.Partner)
                  .HasForeignKey(bap => bap.PartnerId);
                  
            builder.HasMany(p => p.ContactPartners)
                  .WithOne(cp => cp.Partner)
                  .HasForeignKey(cp => cp.PartnerId);
                  
            builder.HasMany(p => p.AddressPartners)
                  .WithOne(ap => ap.Partner)
                  .HasForeignKey(ap => ap.PartnerId);
                  
            builder.HasMany(p => p.LedgerEntries)
                  .WithOne(le => le.Partner)
                  .HasForeignKey(le => le.PartnerId);
        }
    }
}
