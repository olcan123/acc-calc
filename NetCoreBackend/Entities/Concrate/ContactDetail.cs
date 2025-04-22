using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class ContactDetail : BaseEntity
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrimary { get; set; }

        public Contact Contact { get; set; }
    }
}