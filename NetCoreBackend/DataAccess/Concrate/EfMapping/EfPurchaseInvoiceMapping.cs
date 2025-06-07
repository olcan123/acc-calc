using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfPurchaseInvoiceMapping : EfBaseEntityMapping<PurchaseInvoice>
    {
        public override void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(p => p.Id);

            // Property configurations
            builder.Property(p => p.InvoiceType).HasConversion<int>();
            builder.Property(p => p.InvoiceNo).IsRequired().HasMaxLength(50);
            builder.Property(p => p.InvoiceDate).IsRequired();
            builder.Property(p => p.ImportPartnerDocNo).HasMaxLength(50);
            builder.Property(p => p.ImportPartnerDocDate);
            builder.Property(p => p.ExchangeRate).HasColumnType("decimal(18,6)");
            builder.Property(p => p.Status).HasDefaultValue(0);
            builder.Property(p => p.Note).HasMaxLength(500);
            builder.Property(p => p.IsPaid).HasDefaultValue(false);
            builder.Property(p => p.CashPaymentAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);

            // Relationships
            builder.HasOne(p => p.Partner)
                  .WithMany(p => p.PurchaseInvoices)
                  .HasForeignKey(p => p.PartnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Currency)
                  .WithMany(p => p.PurchaseInvoices)
                  .HasForeignKey(p => p.CurrencyId)
                  .OnDelete(DeleteBehavior.Restrict);            builder.HasOne(p => p.Ledger)
                  .WithMany(p => p.PurchaseInvoices)
                  .HasForeignKey(p => p.LedgerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Collections
            builder.HasMany(p => p.PurchaseInvoiceLines)
                  .WithOne(l => l.PurchaseInvoice)
                  .HasForeignKey(l => l.PurchaseInvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PurchaseInvoiceExpenses)
                  .WithOne(e => e.PurchaseInvoice)
                  .HasForeignKey(e => e.PurchaseInvoiceId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
