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
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //Vergiler
        public float CustomsTaxRate { get; set; } // Gümrük Vergisi Oranı
        public float ExciseTaxRate { get; set; } // ÖTV Oranı
        public int VatId { get; set; }
        public Vat Vat { get; set; } // KDV Oranı

        public int UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }



        public ICollection<Barcode> Barcodes { get; set; }             // 1 → N
        public ICollection<ProductCategory> ProductCategories { get; set; } // N → N
        public ICollection<ProductPrice> ProductPrices { get; set; }          // 1 → N
        public ICollection<ProductImage> ProductImages { get; set; }   // 1 → N
        public ICollection<ProductDocument> ProductDocuments { get; set; } // 1 → N

    }
}
