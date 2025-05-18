using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class AccountType : BaseEntity
    {
        public AccountType()
        {
            Accounts = new HashSet<Account>();
        }
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDebitBalance { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}