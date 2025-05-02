using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfContactWarehouseMapping : EfBaseEntityMapping<ContactWarehouse>
    {
        public override void Configure(EntityTypeBuilder<ContactWarehouse> builder)
        {
            base.Configure(builder);

            builder.HasKey(cw => new { cw.ContactId, cw.WarehouseId });

            builder.HasOne(cw => cw.Contact).WithMany(c => c.ContactWarehouses).HasForeignKey(cw => cw.ContactId);

            builder.HasOne(cw => cw.Warehouse).WithMany(w => w.ContactWarehouses).HasForeignKey(cw => cw.WarehouseId);
        }
    }
}