using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class LedgerEntry : BaseEntity
    {
        public int Id { get; set; }
        public int LedgerId { get; set; }
        public int PartnerId { get; set; }
        public int LineNo { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyRate { get; set; }
        public decimal DebitFc { get; set; } // Foreign Currency
        public decimal CreditFc { get; set; } // Foreign Currency
        public Ledger Ledger { get; set; }
        public Account Account { get; set; }
        public Currency Currency { get; set; }
        public Partner Partner { get; set; } = null;
    }
}