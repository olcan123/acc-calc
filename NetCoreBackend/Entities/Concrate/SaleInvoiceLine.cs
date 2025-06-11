using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class SaleInvoiceLine : BaseEntity
    {
        public int Id { get; set; }
        public int SaleInvoiceId { get; set; } // İlişkili Fatura ID
        public int ProductId { get; set; } // Ürün ID
        public int WarehouseId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int SaleAccountId { get; set; } // Satış Hesap Kodu
        public int VatId { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; } // Tutar (Quantity * UnitPrice)
        public decimal DiscountAmount { get; set; } // İndirim Tutarı (DiscountRate * Amount)
        public decimal VatTaxAmount { get; set; } // KDV Tutarı
        public decimal TotalPrice { get; set; } // Toplam Fiyat (Quantity * UnitPrice)
        public decimal TotalAmount { get; set; } // Toplam Tutar (Amount - DiscountAmount + VatTaxAmount)
        public string Description { get; set; } // Açıklama

        public SaleInvoice SaleInvoice { get; set; } = null!; // İlişkili Fatura
        public Product Product { get; set; } = null!; // İlişkili Ürün  
        public Warehouse Warehouse { get; set; } = null!; // İlişkili Depo
        public UnitOfMeasure UnitOfMeasure { get; set; } = null!; // İlişkili Ölçü Birimi
        public Vat Vat { get; set; } = null!; // İlişkili KDV
        public Account SaleAccount { get; set; } = null!; // İlişkili Satış Hesabı



    }
}