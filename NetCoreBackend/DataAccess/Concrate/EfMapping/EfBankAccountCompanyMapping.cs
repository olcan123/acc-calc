using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfBankAccountCompanyMapping : EfBaseEntityMapping<BankAccountCompany>
    {
        public override void Configure(EntityTypeBuilder<BankAccountCompany> builder)
        {
            base.Configure(builder);

            builder.HasKey(bac => new { bac.BankAccountId, bac.CompanyId });

            builder.HasOne(bac => bac.BankAccount).WithMany(ba => ba.BankAccountCompanies).HasForeignKey(bac => bac.BankAccountId);

            builder.HasOne(bac => bac.Company).WithMany(c => c.BankAccountCompanies).HasForeignKey(bac => bac.CompanyId);
        }
    }
}