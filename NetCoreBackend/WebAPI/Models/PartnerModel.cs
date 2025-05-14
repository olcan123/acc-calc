using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;

namespace WebAPI.Models
{
    public class PartnerModel
    {
        
    }

    public class PartnerWithAddressModel
    {
        public Address Address { get; set; }
        public Partner Partner { get; set; }
    }

    public class PartnerContactModel
    {
        public Contact Contact { get; set; }
        public List<ContactDetail> ContactDetails { get; set; }
    }
}