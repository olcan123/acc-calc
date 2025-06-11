using System;
using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class Product : BaseEntity
    {
        public Product()
        {
            Barcodes = new HashSet<Barcode>();
            ProductCategories = new HashSet<ProductCategory>();
            ProductPrices = new HashSet<ProductPrice>();
            ProductImages = new HashSet<ProductImage>();
            ProductDocuments = new HashSet<ProductDocument>();
            PurchaseInvoiceLines = new HashSet<PurchaseInvoiceLine>();
            SaleInvoiceLines = new HashSet<SaleInvoiceLine>();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int PurchaseAccountId { get; set; } // Alım Hesabı
        public int SaleAccountId { get; set; } // Satış Hesabı

        //Vergiler
        public float CustomsTaxRate { get; set; } // Gümrük Vergisi Oranı
        public float ExciseTaxRate { get; set; } // ÖTV Oranı
        public int VatId { get; set; }
        public Vat Vat { get; set; } // KDV Oranı


        public ProductType ProductType { get; set; }

        public int UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public Account PurchaseAccount { get; set; } // Alım Hesabı
        public Account SaleAccount { get; set; } // Satış Hesabı

        public ICollection<Barcode> Barcodes { get; set; }             // 1 → N
        public ICollection<ProductCategory> ProductCategories { get; set; } // N → N
        public ICollection<ProductPrice> ProductPrices { get; set; }          // 1 → N
        public ICollection<ProductImage> ProductImages { get; set; }   // 1 → N
        public ICollection<ProductDocument> ProductDocuments { get; set; } // 1 → N
        public ICollection<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; } 
        public ICollection<SaleInvoiceLine> SaleInvoiceLines { get; set; } = new HashSet<SaleInvoiceLine>(); // 1 → N

    }

    public enum ProductType : short
    {
        StockableMerchandise = 1,
        RawMaterial = 2,
        WorkInProgress = 3,
        FinishedGoods = 4,
        Service = 5,
        FixedAsset = 7,
        Expense = 8,
        Advance = 9,
    }
}
