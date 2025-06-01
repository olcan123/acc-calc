using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfBankAccountMapping : EfBaseEntityMapping<BankAccount>
    {
        public override void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(ba => ba.Id);

            // Property configurations
            builder.Property(ba => ba.BranchName).HasMaxLength(100);
            builder.Property(ba => ba.AccountNumber).HasMaxLength(50);
            builder.Property(ba => ba.IBAN).HasMaxLength(50);
            builder.Property(ba => ba.SwiftCode).HasMaxLength(20);
            
            // Relationships
            builder.HasOne(ba => ba.Bank)
                  .WithMany()
                  .HasForeignKey(ba => ba.BankId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            builder.HasOne(ba => ba.Currency)
                  .WithMany(c => c.BankAccounts)
                  .HasForeignKey(ba => ba.CurrencyId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            builder.HasMany(ba => ba.BankAccountCompanies)
                  .WithOne(bac => bac.BankAccount)
                  .HasForeignKey(bac => bac.BankAccountId);
                  
            builder.HasMany(ba => ba.BankAccountPartners)
                  .WithOne(bap => bap.BankAccount)
                  .HasForeignKey(bap => bap.BankAccountId);
        }
    }
}
