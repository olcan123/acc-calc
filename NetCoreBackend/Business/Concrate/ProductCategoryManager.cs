using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class ProductCategoryManager : IProductCategoryService
    {
        private readonly IProductCategoryDal _productCategoryDal;
        public ProductCategoryManager(IProductCategoryDal productCategoryDal)
        {
            _productCategoryDal = productCategoryDal;
        }

        public IDataResult<ProductCategory> GetById(int productId, int categoryId)
        {
            var productCategory = _productCategoryDal.Get(p => p.ProductId == productId && p.CategoryId == categoryId);
            return new SuccessDataResult<ProductCategory>(productCategory);
        }

        public IDataResult<List<ProductCategory>> GetList()
        {
            var productCategories = _productCategoryDal.GetAll();
            return new SuccessDataResult<List<ProductCategory>>(productCategories);
        }

        public IDataResult<List<ProductCategory>> GetListByCategoryId(int categoryId)
        {
            var productCategories = _productCategoryDal.GetAll(p => p.CategoryId == categoryId);
            return new SuccessDataResult<List<ProductCategory>>(productCategories);
        }

        public IDataResult<List<ProductCategory>> GetListByProductId(int productId)
        {
            var productCategories = _productCategoryDal.GetAll(p => p.ProductId == productId);
            return new SuccessDataResult<List<ProductCategory>>(productCategories);
        }


        public IResult Add(ProductCategory productCategory)
        {
            _productCategoryDal.Add(productCategory);
            return new SuccessResult("Ürün Kategorisi Eklendi");
        }

        public IResult Delete(ProductCategory productCategory)
        {
            _productCategoryDal.Delete(productCategory);
            return new SuccessResult("Ürün Kategorisi Silindi");
        }


        public IResult Update(ProductCategory productCategory)
        {
            _productCategoryDal.Update(productCategory);
            return new SuccessResult("Ürün Kategorisi Güncellendi");
        }

        //SECTION - Bulk Add, Update, Delete
        public IResult BulkAdd(List<ProductCategory> productCategories)
        {
            _productCategoryDal.BulkAdd(productCategories);
            return new SuccessResult("Ürün Kategorileri Eklendi");
        }

        public IResult BulkAddOrUpdate(List<ProductCategory> productCategories)
        {
            _productCategoryDal.BulkAddOrUpdate(productCategories);
            return new SuccessResult("Ürün Kategorileri Güncellendi");
        }

        public IResult BulkDelete(List<ProductCategory> productCategories)
        {
            _productCategoryDal.BulkDelete(productCategories);
            return new SuccessResult("Ürün Kategorileri Silindi");
        }
    }
}