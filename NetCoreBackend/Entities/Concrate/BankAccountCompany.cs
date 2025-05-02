using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class BankAccountCompany : BaseEntity
    {
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}