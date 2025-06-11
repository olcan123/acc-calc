using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class SaleInvoice : BaseEntity
    {
        public SaleInvoice()
        {
            SaleInvoiceLines = new HashSet<SaleInvoiceLine>();
        }

        public int Id { get; set; }
        public int LedgerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public int PartnerId { get; set; }
        public int CustomerAccountId { get; set; } // Müşteri Hesap Kodu
        public string Note { get; set; }
        public bool IsPaid { get; set; }
        public decimal CashPaymentAmount { get; set; }
        public bool IsWholeSale { get; set; } // Toptan Satış mı?
        public SaleInvoiceType SaleInvoiceType { get; set; }

        public ICollection<SaleInvoiceLine> SaleInvoiceLines { get; set; } // Fatura Satırları

        public Ledger Ledger { get; set; } // İlişkili Defter
        public Partner Partner { get; set; } // İlişkili Partner (Müşteri)
        public Account CustomerAccount { get; set; } // İlişkili Müşteri Hesabı
    }

    public enum SaleInvoiceType : short // The underlying type can be short or int.
    {
        LocalInvoice = 1,    // Yerel Mal Satış Faturası
        ExportInvoice = 2,
        DebitNote = 3,       // Verilen fiyat farkı / Borç Dekontu (Müşteriye)
        CreditNote = 4,      // Alınan fiyat farkı / Alacak Dekontu (Müşteriden)
    }
}