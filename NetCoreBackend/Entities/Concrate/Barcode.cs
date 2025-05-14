using Core.Entities;

namespace Entities.Concrate
{
    public class Barcode : BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
