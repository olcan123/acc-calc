using System;
using Core.Entities;

namespace Entities.Concrate
{
    public class PurchaseInvoiceExpense : BaseEntity
    {
        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public int PartnerId { get; set; }
        public string PartnerInvoiceNo { get; set; }
        public DateTime PartnerInvoiceDate { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public decimal RevaluationAmount { get; set; } // Yeniden Değerleme Tutarı
        public decimal Amount { get; set; }
        public decimal AmountFc { get; set; }
        public bool IsPaid { get; set; }
        public decimal CashPaymentAmount { get; set; }
        public PurchaseInvoice PurchaseInvoice { get; set; }
        public Partner Partner { get; set; }
    }

    public enum ExpenseType : short // Temel tip olarak short kalabilir veya int olabilir.
    {
        Freight = 1,         // Nakliye
        Insurance = 2,         // Sigorta
        CustomsExpense = 3,   // Customs Expense (Vergi dışı gümrük işlemi masrafı)
        OtherExpense = 99  // Other Expenses
        // İhtiyaca göre buraya yeni masraf türleri eklenebilir.
    }
}