using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IProductPriceService
    {
        IDataResult<List<ProductPrice>> GetList();
        IDataResult<List<ProductPrice>> GetListByProductId(int productId);
        IDataResult<ProductPrice> GetById(int id);
        IDataResult<ProductPrice> GetByIdInclude(int id);
        IResult Add(ProductPrice productPrice);
        IResult Update(ProductPrice productPrice);
        IResult Delete(ProductPrice productPrice);

        //SECTION - BULK OPERATIONS
        IResult BulkAdd(List<ProductPrice> productPrices);
        IResult BulkDelete(List<ProductPrice> productPrices);
        IResult BulkAddOrUpdate(List<ProductPrice> productPrices);
    }
}
