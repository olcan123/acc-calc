using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfContactPartnerMapping : EfBaseEntityMapping<ContactPartner>
    {
        public override void Configure(EntityTypeBuilder<ContactPartner> builder)
        {
            base.Configure(builder);

            builder.HasKey(cp => new { cp.ContactId, cp.PartnerId });

            builder.HasOne(cp => cp.Contact).WithMany(c => c.ContactPartners).HasForeignKey(cp => cp.ContactId);

            builder.HasOne(cp => cp.Partner).WithMany(p => p.ContactPartners).HasForeignKey(cp => cp.PartnerId);
        }
    }
}