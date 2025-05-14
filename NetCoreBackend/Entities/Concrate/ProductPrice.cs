using System;
using Core.Entities;

namespace Entities.Concrate
{
    /// <summary>Fiyatın kategorisi (“Liste” mi, “Promosyon” mu?)</summary>
    public enum PriceCategory : byte
    {
        Regular = 0,   // Standart liste fiyatı
        Promo = 1    // Süreli promosyon fiyatı
    }

    /// <summary>Fiyatın yönü — alış maliyeti mi, satış fiyatı mı?</summary>
    public enum PriceSide : byte
    {
        Purchase = 0,  // Alış (tedarikçi maliyeti)
        Sales = 1   // Satış (müşteriye)
    }

    public class ProductPrice : BaseEntity
    {
        public int Id { get; set; }

        // —— İlişki: Product 1 → N ProductPrice ——
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // —— Asıl fiyat bilgisi ——
        public decimal UnitPrice { get; set; }
        public PriceCategory Category { get; set; }  // Regular / Promo
        public PriceSide Side { get; set; }   // Purchase / Sales

        /// <remarks>
        /// Sadece Category = Promo iken geçerli olmalı.
        /// </remarks>
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
