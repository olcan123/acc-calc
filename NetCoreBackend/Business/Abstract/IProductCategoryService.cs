using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IProductCategoryService
    {
        IDataResult<List<ProductCategory>> GetList();
        IDataResult<List<ProductCategory>> GetListByProductId(int productId);
        IDataResult<List<ProductCategory>> GetListByCategoryId(int categoryId);
        IDataResult<ProductCategory> GetById(int productId, int categoryId);
        IResult Add(ProductCategory productCategory);
        IResult Update(ProductCategory productCategory);
        IResult Delete(ProductCategory productCategory);

        IResult BulkAdd(List<ProductCategory> productCategories);
        IResult BulkDelete(List<ProductCategory> productCategories);
        IResult BulkAddOrUpdate(List<ProductCategory> productCategories);
    }
}
