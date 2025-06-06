using Core.Entities;

namespace Entities.Concrate
{
    public class ProductCategory : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
