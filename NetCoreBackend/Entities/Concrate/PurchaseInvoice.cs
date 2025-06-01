using System;
using System.Collections.Generic;
using Core.Entities;
using Entities.Concrate;

namespace Entities.Concrate
{

    public class PurchaseInvoice : BaseEntity
    {
        public PurchaseInvoice()
        {
            PurchaseInvoiceLines = new HashSet<PurchaseInvoiceLine>();
            PurchaseInvoiceExpenses = new HashSet<PurchaseInvoiceExpense>();
        }
        public int Id { get; set; }
        public int LedgerId { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public int PartnerId { get; set; }
        public int VendorAccountId { get; set; } // Tedarikçi Hesap Kodu
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ImportPartnerDocNo { get; set; }
        public DateTime? ImportPartnerDocDate { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public short Status { get; set; }
        public string Note { get; set; }
        public bool IsPaid { get; set; }
        public decimal CashPaymentAmount { get; set; }


        public Partner Partner { get; set; }
        public Currency Currency { get; set; }
        public Ledger Ledger { get; set; }
        public Account VendorAccount { get; set; } // Tedarikçi Hesapları

        public ICollection<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
        public ICollection<PurchaseInvoiceExpense> PurchaseInvoiceExpenses { get; set; }
    }

    public enum InvoiceType : short // The underlying type can be short or int.
    {
        LocalInvoice = 1,    // Yerel Mal Alım Faturası
        ImportInvoice = 2,
        DebitNote = 3,               // Alınan fiyat farkı / Borç Dekontu (Tedarikçiden)
        CreditNote = 4,              // Verilen fiyat farkı / Alacak Dekontu (Tedarikçiye)
    }
}