using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfBankAccountPartnerMapping : EfBaseEntityMapping<BankAccountPartner>
    {
        public override void Configure(EntityTypeBuilder<BankAccountPartner> builder)
        {
            base.Configure(builder);

            builder.HasKey(bap => new { bap.BankAccountId, bap.PartnerId });

            builder.HasOne(bap => bap.BankAccount).WithMany(ba => ba.BankAccountPartners).HasForeignKey(bap => bap.BankAccountId);

            builder.HasOne(bap => bap.Partner).WithMany(p => p.BankAccountPartners).HasForeignKey(bap => bap.PartnerId);
        }
    }
}