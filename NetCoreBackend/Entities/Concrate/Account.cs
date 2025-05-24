using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;

namespace Entities.Concrate
{

    public class Account : BaseEntity
    {


        public Account()
        {
            Children = new HashSet<Account>();
            LedgerEntries = new HashSet<LedgerEntry>();
            PurchaseInvoiceLines = new HashSet<PurchaseInvoiceLine>();
            // Koleksiyonlar burada başlatılır eğer varsa
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ParentAccountId { get; set; } // Null olabilir (en üst seviye hesaplar için)
        public bool IsActive { get; set; } = true; // Varsayılan olarak aktif
        public bool IsPostable { get; set; } = true; // Varsayılan olarak kayıt yapılabilir
        public NormalBalanceOption? NormalBalance { get; set; } // Enum kullanımı, null olabilir
        public AccountTypeOption AccountType { get; set; } // Enum kullanımı
        // Uzun metinler için
        public string Description { get; set; }

        // Diğer tablolarla ilişkiler için navigasyon özellikleri
        public Account ParentAccount { get; set; } // Üst hesaba navigasyon özelliği
        public ICollection<Account> Children { get; set; } // Alt hesaplara navigasyon özelliği
        public ICollection<LedgerEntry> LedgerEntries { get; set; }
        public ICollection<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; } = null!;

        public ICollection<Product> PurchaseProducts { get; set; } = null!; // Alım hesapları için ürünler
        public ICollection<Product> SaleProducts { get; set; } = null!; // Satış hesapları için ürünler
    }

    // Hesap türleri için enum tanımı
    public enum AccountTypeOption
    {
        Asset = 1,
        Liability = 2,
        Equity = 3,
        Revenue = 4,
        Expense = 5,
        ContraAsset = 6,
        ContraLiability = 7,
        ContraEquity = 8,
        ContraRevenue = 9,
        ContraExpense = 10,
    }

    // Normal bakiye türleri için enum tanımı
    public enum NormalBalanceOption
    {
        Debit = 1,
        Credit = 2
    }
}