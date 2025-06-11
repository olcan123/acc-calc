using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class Ledger : BaseEntity
    {
        public Ledger()
        {
            LedgerEntries = new HashSet<LedgerEntry>();
            PurchaseInvoices = new HashSet<PurchaseInvoice>();
            SaleInvoices = new HashSet<SaleInvoice>();
        }
        public int Id { get; set; }
        public short DocumentType { get; set; }
        public DateTime DocumentDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Description { get; set; }
        public short Status { get; set; }
        public ICollection<LedgerEntry> LedgerEntries { get; set; }
        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
        public ICollection<SaleInvoice> SaleInvoices { get; set; }
    }

    public enum LedgerDocumentType
    {
        PurchaseInvoice = 1,
        SalesInvoice = 2,
        Payment = 3,
        Receipt = 4,
        Journal = 5
    }
}