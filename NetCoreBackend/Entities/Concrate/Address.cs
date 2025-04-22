using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class Address : BaseEntity
    {

        public Address()
        {
            Warehouses = new HashSet<Warehouse>();
            Partners = new HashSet<Partner>();
        }
        public int Id { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public ICollection<Warehouse> Warehouses { get; set; }
        public ICollection<Partner> Partners { get; set; }
    }
}