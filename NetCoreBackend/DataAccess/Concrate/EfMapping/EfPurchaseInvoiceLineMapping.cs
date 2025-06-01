using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfPurchaseInvoiceLineMapping : EfBaseEntityMapping<PurchaseInvoiceLine>
    {
        public override void Configure(EntityTypeBuilder<PurchaseInvoiceLine> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(p => p.Id);

            // Property configurations
            builder.Property(p => p.Quantity).HasColumnType("decimal(18,6)");
            builder.Property(p => p.UnitPrice).HasColumnType("decimal(18,6)");
            builder.Property(p => p.Amount).HasColumnType("decimal(18,6)");
            builder.Property(p => p.ExpenseAmount).HasColumnType("decimal(18,6)");
            builder.Property(p => p.DiscountRate).HasColumnType("decimal(18,6)");
            builder.Property(p => p.DiscountAmount).HasColumnType("decimal(18,6)");
            builder.Property(p => p.ExciseTaxRate).HasColumnType("decimal(18,6)");
            builder.Property(p => p.ExciseTaxAmount).HasColumnType("decimal(18,6)");
            builder.Property(p => p.CustomsRate).HasColumnType("decimal(18,6)");
            builder.Property(p => p.CustomsAmount).HasColumnType("decimal(18,6)");
            builder.Property(p => p.RevaluationAmount).HasColumnType("decimal(18,6)");
            builder.Property(p => p.VatTaxAmount).HasColumnType("decimal(18,6)");
            builder.Property(p => p.CostPrice).HasColumnType("decimal(18,6)");
            builder.Property(p => p.CostAmount).HasColumnType("decimal(18,6)");
            builder.Property(p => p.TotalPrice).HasColumnType("decimal(18,6)");
            builder.Property(p => p.TotalAmount).HasColumnType("decimal(18,6)");

            // Relationships
            builder.HasOne(p => p.PurchaseInvoice)
                  .WithMany(i => i.PurchaseInvoiceLines)
                  .HasForeignKey(p => p.PurchaseInvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Product)
                  .WithMany(p => p.PurchaseInvoiceLines)
                  .HasForeignKey(p => p.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Warehouse)
                  .WithMany(p=>p.PurchaseInvoiceLines)
                  .HasForeignKey(p => p.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.UnitOfMeasure)
                  .WithMany(p=>p.PurchaseInvoiceLines)
                  .HasForeignKey(p => p.UnitOfMeasureId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Vat)
                  .WithMany(p=> p.PurchaseInvoiceLines)
                  .HasForeignKey(p => p.VatId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.PurchaseAccount)
                  .WithMany(p=>p.PurchaseInvoiceLines)
                  .HasForeignKey(p => p.PurchaseAccountId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
