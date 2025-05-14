using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;

namespace WebAPI.Models
{
    public class ProductModel
    {

    }

    public class ProductOtherEntitiesModel
    {
        public Product Product { get; set; }
        public List<Barcode> Barcodes { get; set; }
        public List<ProductPrice> ProductPrices { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}