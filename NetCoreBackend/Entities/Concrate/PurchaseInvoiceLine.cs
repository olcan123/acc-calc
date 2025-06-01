using System;
using System.Collections.Generic;
using Core.Entities;
using Entities.Concrate;

namespace Entities.Concrate
{
    public class PurchaseInvoiceLine : BaseEntity
    {
        public PurchaseInvoiceLine()
        {
            // Koleksiyonlar burada başlatılır eğer varsa
        }

        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int VatId { get; set; }
        public int PurchaseAccountId { get; set; }
        public decimal Quantity { get; set; }

        //SECTION - Faturadaki degerleri
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }


        public decimal ExpenseAmount { get; set; }

        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }


        public decimal ExciseTaxRate { get; set; }
        public decimal ExciseTaxAmount { get; set; }
        public decimal CustomsRate { get; set; }
        public decimal CustomsAmount { get; set; }
        public decimal RevaluationAmount { get; set; } // Yeniden Değerleme Tutarı
        public decimal VatTaxAmount { get; set; }

        public decimal CostPrice { get; set; }
        public decimal CostAmount { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal TotalAmount { get; set; }

        public PurchaseInvoice PurchaseInvoice { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
        public UnitOfMeasure UnitOfMeasure { get; set; } = null!;
        public Vat Vat { get; set; } = null!;
        public Account PurchaseAccount { get; set; } = null!;

    }
}