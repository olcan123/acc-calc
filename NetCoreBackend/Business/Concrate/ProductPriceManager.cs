using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class ProductPriceManager : IProductPriceService
    {
        private readonly IProductPriceDal _productPriceDal;

        public ProductPriceManager(IProductPriceDal productPriceDal)
        {
            _productPriceDal = productPriceDal;
        }

        public IDataResult<List<ProductPrice>> GetList()
        {
            var result = _productPriceDal.GetAll();
            return new SuccessDataResult<List<ProductPrice>>(result);
        }

        public IDataResult<List<ProductPrice>> GetListByProductId(int productId)
        {
            var result = _productPriceDal.GetAll(p => p.ProductId == productId);
            return new SuccessDataResult<List<ProductPrice>>(result);
        }

        public IDataResult<ProductPrice> GetById(int id)
        {
            var result = _productPriceDal.Get(x => x.Id == id);
            return new SuccessDataResult<ProductPrice>(result);
        }

        public IDataResult<ProductPrice> GetByIdInclude(int id)
        {
            var result = _productPriceDal.GetWithIncludeChain(
                x => x.Include(pp => pp.Product),
                x => x.Id == id);
            return new SuccessDataResult<ProductPrice>(result);
        }

        [ValidationAspect(typeof(ProductPriceValidator), Priority = 1)]
        public IResult Add(ProductPrice productPrice)
        {
            _productPriceDal.Add(productPrice);
            return new SuccessResult("Ürün Fiyatı Eklendi");
        }

        public IResult Delete(ProductPrice productPrice)
        {
            _productPriceDal.Delete(productPrice);
            return new SuccessResult("Ürün Fiyatı Silindi");
        }

        [ValidationAspect(typeof(ProductPriceValidator), Priority = 1)]
        public IResult Update(ProductPrice productPrice)
        {
            _productPriceDal.Update(productPrice);
            return new SuccessResult("Ürün Fiyatı Güncellendi");
        }

        [ValidationAspect(typeof(ProductPriceValidator), Priority = 1)]
        public IResult BulkAdd(List<ProductPrice> productPrices)
        {
            _productPriceDal.BulkAdd(productPrices);
            return new SuccessResult("Ürün Fiyatları toplu olarak eklendi");
        }

        public IResult BulkDelete(List<ProductPrice> productPrices)
        {
            _productPriceDal.BulkDelete(productPrices);
            return new SuccessResult("Ürün Fiyatları toplu olarak silindi");
        }

        [ValidationAspect(typeof(ProductPriceValidator), Priority = 1)]
        public IResult BulkAddOrUpdate(List<ProductPrice> productPrices)
        {
            _productPriceDal.MergeLinq(productPrices, (source, target) => source.Id == target.Id);
            return new SuccessResult("Ürün Fiyatları toplu olarak eklendi veya güncellendi");
        }
    }
}
