using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class Address : BaseEntity
    {

        public Address()
        {
            AddressWarehouses = new HashSet<AddressWarehouse>();
            AddressPartners = new HashSet<AddressPartner>();
        }
        public int Id { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; } 
        public ICollection<AddressWarehouse> AddressWarehouses { get; set; }
        public ICollection<AddressPartner> AddressPartners { get; set; }
    }
}