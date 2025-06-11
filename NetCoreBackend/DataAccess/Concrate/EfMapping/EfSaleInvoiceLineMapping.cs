using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfSaleInvoiceLineMapping : EfBaseEntityMapping<SaleInvoiceLine>
    {
        public override void Configure(EntityTypeBuilder<SaleInvoiceLine> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(sl => sl.Id);

            // Property configurations
            builder.Property(sl => sl.Quantity).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(sl => sl.UnitPrice).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(sl => sl.Amount).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(sl => sl.DiscountRate).HasColumnType("decimal(18,4)").HasDefaultValue(0);
            builder.Property(sl => sl.DiscountAmount).HasColumnType("decimal(18,4)").HasDefaultValue(0);
            builder.Property(sl => sl.VatTaxAmount).HasColumnType("decimal(18,4)").HasDefaultValue(0);
            builder.Property(sl => sl.TotalPrice).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(sl => sl.Description).HasMaxLength(500);

            // Relationships
            builder.HasOne(sl => sl.SaleInvoice)
                  .WithMany(s => s.SaleInvoiceLines)
                  .HasForeignKey(sl => sl.SaleInvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sl => sl.Product)
                  .WithMany(sl => sl.SaleInvoiceLines)
                  .HasForeignKey(sl => sl.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sl => sl.Warehouse)
                  .WithMany(sl => sl.SaleInvoiceLines)
                  .HasForeignKey(sl => sl.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sl => sl.UnitOfMeasure)
                  .WithMany(s => s.SaleInvoiceLines)
                  .HasForeignKey(sl => sl.UnitOfMeasureId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sl => sl.Vat)
                  .WithMany(sl => sl.SaleInvoiceLines)
                  .HasForeignKey(sl => sl.VatId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sl => sl.SaleAccount)
                  .WithMany(sl => sl.SaleInvoiceLines)
                  .HasForeignKey(sl => sl.SaleAccountId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
