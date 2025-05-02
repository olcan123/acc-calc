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
            BankAccountCompanies = new HashSet<BankAccountCompany>();
            Warehouses = new HashSet<Warehouse>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string UidNumber { get; set; }
        public string VatNumber { get; set; }
        public string Period { get; set; }

        public ICollection<BankAccountCompany> BankAccountCompanies { get; set; }
        public ICollection<Warehouse> Warehouses { get; set; }

    }
}