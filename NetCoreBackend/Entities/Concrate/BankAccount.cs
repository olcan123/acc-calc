using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class BankAccount : BaseEntity
    {
        public BankAccount()
        {
            Companies = new HashSet<Company>();
            Contacts = new HashSet<Contact>();
            Partners = new HashSet<Partner>();
        }

        public int Id { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string Currency { get; set; }

        public Bank Bank { get; set; } // Navigation property

        public ICollection<Company> Companies { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Partner> Partners { get; set; }
    }

}