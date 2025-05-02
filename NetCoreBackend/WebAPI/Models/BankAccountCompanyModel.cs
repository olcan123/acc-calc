using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;

namespace WebAPI.Models
{
    public class BankAccountCompanyModel
    {
        public BankAccount BankAccount { get; set; }
        public BankAccountCompany BankAccountCompany { get; set; }
    }
}