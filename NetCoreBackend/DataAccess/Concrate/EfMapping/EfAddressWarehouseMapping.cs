using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfAddressWarehouseMapping : EfBaseEntityMapping<AddressWarehouse>
    {
        public override void Configure(EntityTypeBuilder<AddressWarehouse> builder)
        {
            base.Configure(builder);

            builder.HasKey(aw => new { aw.AddressId, aw.WarehouseId });

            builder.HasOne(aw => aw.Address).WithMany(a => a.AddressWarehouses).HasForeignKey(aw => aw.AddressId);

            builder.HasOne(aw => aw.Warehouse).WithMany(w => w.AddressWarehouses).HasForeignKey(aw => aw.WarehouseId);
        }
    }
}