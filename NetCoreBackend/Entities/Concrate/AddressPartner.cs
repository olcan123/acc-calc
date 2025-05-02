using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class AddressPartner : BaseEntity
    {
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int PartnerId { get; set; }
        public Partner Partner { get; set; }
    }
}