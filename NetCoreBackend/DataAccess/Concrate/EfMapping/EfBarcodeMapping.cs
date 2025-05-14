using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfBarcodeMapping : EfBaseEntityMapping<Barcode>
    {
        public override void Configure(EntityTypeBuilder<Barcode> builder)
        {
            base.Configure(builder);

            // EF Core 9 uyumlu CHECK constraint
            builder
                .ToTable("Barcodes", t => t.HasCheckConstraint("CK_Barcodes_Id_Positive", "\"Id\" > 0"));

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                   .ValueGeneratedOnAdd(); // Auto-increment destekli

            builder.Property(b => b.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.Type)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.ProductId).IsRequired();

            builder.HasOne(b => b.Product)
                   .WithMany(p => p.Barcodes)
                   .HasForeignKey(b => b.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}