using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetList();
        IDataResult<List<Product>> GetListInclude();
        IDataResult<Product> GetById(int id);
        IDataResult<Product> GetByIdInclude(int id);
        IResult Add(Product product);
        IResult Update(Product product);
        IResult Delete(Product product);

        //SECTION - Add Product,Barcodes,Prices,ProductCategories
        IResult AddProductWithOtherEntities(Product product,
            List<Barcode> barcodes,
            List<ProductPrice> productPrices,
            List<ProductCategory> productCategories);

        IResult DeleteProductWithOtherEntities(int id);

        IResult UpdateProductWithOtherEntities(Product product,
            List<Barcode> barcodes,
            List<ProductPrice> productPrices,
            List<ProductCategory> productCategories);
    }
}
