using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class ContactPartner : BaseEntity
    {
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public int PartnerId { get; set; }
        public Partner Partner { get; set; }
    }
}