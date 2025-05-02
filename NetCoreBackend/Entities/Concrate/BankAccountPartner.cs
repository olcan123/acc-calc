using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class BankAccountPartner : BaseEntity
    {
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public int PartnerId { get; set; }
        public Partner Partner { get; set; }
    }
}