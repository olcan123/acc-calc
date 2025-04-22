using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class Contact : BaseEntity
    {
        public Contact()
        {
            ContactDetails = new HashSet<ContactDetail>();
            Warehouses = new HashSet<Warehouse>();
            Partners = new HashSet<Partner>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ContactDetail> ContactDetails { get; set; }
        public ICollection<Warehouse> Warehouses { get; set; }
        public ICollection<Partner> Partners { get; set; }
    }
}