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
            BankAccountCompanies = new HashSet<BankAccountCompany>();
            BankAccountPartners = new HashSet<BankAccountPartner>();
        }

        public int Id { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public int CurrencyId { get; set; }

        public Bank Bank { get; set; } // Navigation property
        public Currency Currency { get; set; } // Navigation property

        public ICollection<BankAccountCompany> BankAccountCompanies { get; set; }
        public ICollection<BankAccountPartner> BankAccountPartners { get; set; }
    }

}