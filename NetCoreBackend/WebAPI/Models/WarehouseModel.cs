using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;

namespace WebAPI.Models
{
    public class WarehouseAddressModel
    {
        public Warehouse Warehouse { get; set; }
        public List<Address> Addresses { get; set; }
    }

    public class WarehouseContactModel
    {
        public Contact Contact { get; set; }
        public List<ContactDetail> ContactDetails { get; set; }
    }
}