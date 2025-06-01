using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfProductMapping : EfBaseEntityMapping<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(p => p.Id);

            // Property configurations
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.CustomsTaxRate).HasColumnType("decimal(5,2)");
            builder.Property(p => p.ExciseTaxRate).HasColumnType("decimal(5,2)");
            builder.Property(p => p.ProductType).HasConversion<int>();

            // Relationships
            builder.HasOne(p => p.UnitOfMeasure)
                  .WithMany(u => u.Products)
                  .HasForeignKey(p => p.UnitOfMeasureId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Vat)
                  .WithMany(p=> p.Products)
                  .HasForeignKey(p => p.VatId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.PurchaseAccount)
                  .WithMany(p => p.PurchaseProducts)
                  .HasForeignKey(p => p.PurchaseAccountId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.SaleAccount)
                  .WithMany(p => p.SaleProducts)
                  .HasForeignKey(p => p.SaleAccountId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Collections (One to Many relationships)
            builder.HasMany(p => p.Barcodes)
                  .WithOne(b => b.Product)
                  .HasForeignKey(b => b.ProductId);

            builder.HasMany(p => p.ProductPrices)
                  .WithOne(pp => pp.Product)
                  .HasForeignKey(pp => pp.ProductId);

            builder.HasMany(p => p.ProductImages)
                  .WithOne(pi => pi.Product)
                  .HasForeignKey(pi => pi.ProductId);

            builder.HasMany(p => p.ProductDocuments)
                  .WithOne(pd => pd.Product)
                  .HasForeignKey(pd => pd.ProductId);
        }
    }
}
