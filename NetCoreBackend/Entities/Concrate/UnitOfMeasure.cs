using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class UnitOfMeasure : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; } // Kısa Adı

        public ICollection<Product> Products { get; set; } // 1 → N
    }
}
