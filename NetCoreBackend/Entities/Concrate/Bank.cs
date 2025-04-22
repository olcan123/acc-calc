using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class Bank : BaseEntity
    {
        public Bank() => BankAccounts = new HashSet<BankAccount>();

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; }
    }

}