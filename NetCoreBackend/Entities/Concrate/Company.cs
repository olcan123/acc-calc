using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class Company : BaseEntity
    {
        public Company()
        {
            BankAccounts = new HashSet<BankAccount>();
            Warehouses = new HashSet<Warehouse>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string UidNumber { get; set; }
        public string VatNumber { get; set; }
        public string Period { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; }
        public ICollection<Warehouse> Warehouses { get; set; }

    }
}