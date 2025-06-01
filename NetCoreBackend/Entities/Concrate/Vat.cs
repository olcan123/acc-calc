using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrate
{
    public class Vat : BaseEntity
    {

        public Vat()
        {
            Products = new HashSet<Product>();
            PurchaseInvoiceLines = new HashSet<PurchaseInvoiceLine>();
        }

        public int Id { get; set; }
        public string Name { get; set; } // KDV Oranı
        public float Rate { get; set; } // KDV Oranı
        public bool IsDefault { get; set; } // Varsayılan mı?
        public bool IsActive { get; set; } // Aktif mi?

        // 1 → N
        public ICollection<Product> Products { get; set; }
        public ICollection<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; } 
    }
}