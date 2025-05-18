using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class Currency : BaseEntity
    {
        public Currency()
        {
            LedgerEntries = new HashSet<LedgerEntry>();
            PurchaseInvoices = new HashSet<PurchaseInvoice>();
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<LedgerEntry> LedgerEntries { get; set; }
        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
    }
}