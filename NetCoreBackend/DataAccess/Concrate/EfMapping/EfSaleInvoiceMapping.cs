using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfSaleInvoiceMapping : EfBaseEntityMapping<SaleInvoice>
    {
        public override void Configure(EntityTypeBuilder<SaleInvoice> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(s => s.Id);

            // Property configurations
            builder.Property(s => s.SaleInvoiceType).HasConversion<int>();
            builder.Property(s => s.InvoiceNo).IsRequired().HasMaxLength(50);
            builder.Property(s => s.InvoiceDate).IsRequired();
            builder.Property(s => s.Note).HasMaxLength(500);
            builder.Property(s => s.IsPaid).HasDefaultValue(false);
            builder.Property(s => s.CashPaymentAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            builder.Property(s => s.IsWholeSale).HasDefaultValue(false);

            // Relationships
            builder.HasOne(s => s.Partner)
                  .WithMany(p => p.SaleInvoices)
                  .HasForeignKey(s => s.PartnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.CustomerAccount)
                  .WithMany(ca => ca.SaleInvoices)
                  .HasForeignKey(s => s.CustomerAccountId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Ledger)
                  .WithMany(l => l.SaleInvoices)
                    .HasForeignKey(s => s.LedgerId)
                    .OnDelete(DeleteBehavior.Restrict);

            // Collection relationships
            builder.HasMany(s => s.SaleInvoiceLines)
                  .WithOne(sl => sl.SaleInvoice)
                  .HasForeignKey(sl => sl.SaleInvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
