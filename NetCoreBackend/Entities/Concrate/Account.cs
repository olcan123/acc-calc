using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class Account : BaseEntity
    {
        public Account()
        {
            ChildAccounts = new HashSet<Account>();
        }
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public short AccountTypeId { get; set; }
        public int ParentAccountId { get; set; }
        public short Status { get; set; }
        public AccountType AccountType { get; set; }
        public Account ParentAccount { get; set; }
        public ICollection<Account> ChildAccounts { get; set; }
    }
}