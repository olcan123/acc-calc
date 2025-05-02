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
            ContactPartners = new HashSet<ContactPartner>();
            ContactWarehouses = new HashSet<ContactWarehouse>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ContactDetail> ContactDetails { get; set; }
        public ICollection<ContactWarehouse> ContactWarehouses { get; set; }
        public ICollection<ContactPartner> ContactPartners { get; set; }
    }
}