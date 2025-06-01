using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfLedgerEntryMapping : EfBaseEntityMapping<LedgerEntry>
    {
        public override void Configure(EntityTypeBuilder<LedgerEntry> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(le => le.Id);

            // Property configurations
            builder.Property(le => le.LineNo).IsRequired();
            builder.Property(le => le.Description).HasMaxLength(255);
            builder.Property(le => le.Debit).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            builder.Property(le => le.Credit).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            
            // Relationships
            builder.HasOne(le => le.Ledger)
                  .WithMany(l => l.LedgerEntries)
                  .HasForeignKey(le => le.LedgerId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            builder.HasOne(le => le.Account)
                  .WithMany()
                  .HasForeignKey(le => le.AccountId)
                  .OnDelete(DeleteBehavior.Restrict);
                  

            builder.HasOne(le => le.Partner)
                  .WithMany(p => p.LedgerEntries)
                  .HasForeignKey(le => le.PartnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
