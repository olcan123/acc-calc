using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class Warehouse : BaseEntity
    {
        public Warehouse()
        {
            AddressWarehouses = new HashSet<AddressWarehouse>();
            ContactWarehouses = new HashSet<ContactWarehouse>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }

        public Company Company { get; set; }

        public ICollection<AddressWarehouse> AddressWarehouses { get; set; }
        public ICollection<ContactWarehouse> ContactWarehouses { get; set; }
    }
}