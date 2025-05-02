using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfAddressPartnerMapping : EfBaseEntityMapping<AddressPartner>
    {
        public override void Configure(EntityTypeBuilder<AddressPartner> builder)
        {
            base.Configure(builder);

            builder.HasKey(ap => new { ap.AddressId, ap.PartnerId });

            builder.HasOne(ap => ap.Address).WithMany(a => a.AddressPartners).HasForeignKey(ap => ap.AddressId);

            builder.HasOne(ap => ap.Partner).WithMany(p => p.AddressPartners).HasForeignKey(ap => ap.PartnerId);
        }
    }
}