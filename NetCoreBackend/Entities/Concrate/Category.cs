using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrate
{
    public class Category : BaseEntity
    {
        public Category()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } // N → N
    }
}
