using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class ContactWarehouse : BaseEntity
    {
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}