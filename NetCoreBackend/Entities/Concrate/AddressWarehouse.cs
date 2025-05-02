using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class AddressWarehouse : BaseEntity
    {
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}