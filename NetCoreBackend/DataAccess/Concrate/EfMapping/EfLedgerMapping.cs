using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfLedgerMapping : EfBaseEntityMapping<Ledger>
    {
        public override void Configure(EntityTypeBuilder<Ledger> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(l => l.Id);

            // Property configurations
            builder.Property(l => l.DocumentType).HasComment("1=PurchaseInvoice, 2=SalesInvoice, 3=Payment, 4=Receipt, 5=Journal");
            builder.Property(l => l.DocumentDate).IsRequired();
            builder.Property(l => l.ReferenceNo).HasMaxLength(50);
            builder.Property(l => l.Description).HasMaxLength(500);
            builder.Property(l => l.Status).HasDefaultValue(0);
            
            // Relationships
            builder.HasMany(l => l.LedgerEntries)
                  .WithOne(le => le.Ledger)
                  .HasForeignKey(le => le.LedgerId);
                  
            builder.HasMany(l => l.PurchaseInvoices)
                  .WithOne(pi => pi.Ledger)
                  .HasForeignKey(pi => pi.LedgerId);
        }
    }
}
