using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfPurchaseInvoiceExpenseMapping : EfBaseEntityMapping<PurchaseInvoiceExpense>
    {
        public override void Configure(EntityTypeBuilder<PurchaseInvoiceExpense> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(pie => pie.Id);

            // Property configurations
            builder.Property(pie => pie.PartnerInvoiceNo).HasMaxLength(50);
            builder.Property(pie => pie.PartnerInvoiceDate).IsRequired();
            builder.Property(pie => pie.ExpenseType).HasConversion<int>();
            builder.Property(pie => pie.RevaluationAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            builder.Property(pie => pie.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(pie => pie.AmountFc).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            builder.Property(pie => pie.IsPaid).HasDefaultValue(false);
            builder.Property(pie => pie.CashPaymentAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);

            // Relationships
            builder.HasOne(pie => pie.PurchaseInvoice)
                  .WithMany(pi => pi.PurchaseInvoiceExpenses)
                  .HasForeignKey(pie => pie.PurchaseInvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pie => pie.Partner)
                  .WithMany(p => p.PurchaseInvoiceExpenses)
                  .HasForeignKey(pie => pie.PartnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
