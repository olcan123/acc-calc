using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IBarcodeService _barcodeService;
        private readonly IProductPriceService _productPriceService;
        private readonly IProductCategoryService _productCategoryService;

        public ProductManager(IProductDal productDal, IBarcodeService barcodeService, IProductPriceService productPriceService, IProductCategoryService productCategoryService)
        {
            _productDal = productDal;
            _barcodeService = barcodeService;
            _productPriceService = productPriceService;
            _productCategoryService = productCategoryService;
        }

        public IDataResult<List<Product>> GetList()
        {
            var result = _productDal.GetAll();
            return new SuccessDataResult<List<Product>>(result);
        }

        public IDataResult<List<Product>> GetListInclude()
        {
            var result = _productDal.GetAllWithIncludeChain(x => x
                .Include(p => p.Vat)
                .Include(p => p.UnitOfMeasure)
                .Include(p => p.Barcodes)
                .Include(p => p.ProductPrices)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category));

            return new SuccessDataResult<List<Product>>(result);
        }

        public IDataResult<Product> GetById(int id)
        {
            var result = _productDal.Get(x => x.Id == id);
            return new SuccessDataResult<Product>(result);
        }

        public IDataResult<Product> GetByIdInclude(int id)
        {
            var result = _productDal.GetWithIncludeChain(
                x => x
                .Include(p => p.Vat)
                .Include(p => p.UnitOfMeasure)
                .Include(p => p.Barcodes)
                .Include(p => p.ProductPrices)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category),
                x => x.Id == id);

            return new SuccessDataResult<Product>(result);
        }

        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        public IResult Add(Product product)
        {
            _productDal.Add(product);
            return new SuccessResult("Ürün Eklendi");
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult("Ürün Silindi");
        }

        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult("Ürün Güncellendi");
        }

        //SECTION - Add Product,Barcodes,Prices,ProductCategories

        [TransactionScopeAspect]
        public IResult AddProductWithOtherEntities(Product product,
            List<Barcode> barcodes,
            List<ProductPrice> productPrices,
            List<ProductCategory> productCategories)
        {
            _productDal.Add(product);

            if (productPrices != null && productPrices.Any())
            {
                productPrices.ForEach(p => p.ProductId = product.Id);
                _productPriceService.BulkAdd(productPrices);
            }

            if (productCategories != null && productCategories.Any())
            {
                productCategories.ForEach(p => p.ProductId = product.Id);
                _productCategoryService.BulkAdd(productCategories);
            }

            if (barcodes != null && barcodes.Any())
            {
                barcodes.ForEach(p => p.ProductId = product.Id);
                _barcodeService.BulkAdd(barcodes);
            }

            return new SuccessResult("Ürün ve diğer varlıklar eklendi");
        }

        [TransactionScopeAspect]
        public IResult DeleteProductWithOtherEntities(int id)
        {
            var product = _productDal.GetWithIncludeChain(
                x => x
                .Include(p => p.Barcodes)
                .Include(p => p.ProductPrices)
                .Include(p => p.ProductCategories),
                x => x.Id == id);

            if (product == null)
                return new ErrorResult("Ürün bulunamadı");

            //SECTION - Other Entities
            if (product.Barcodes != null && product.Barcodes.Any())
                _barcodeService.BulkDelete(product.Barcodes.ToList());


            if (product.ProductPrices != null && product.ProductPrices.Any())
                _productPriceService.BulkDelete(product.ProductPrices.ToList());

            if (product.ProductCategories != null && product.ProductCategories.Any())
                _productCategoryService.BulkDelete(product.ProductCategories.ToList());

            //SECTION - Product
            _productDal.Delete(new Product { Id = id });

            return new SuccessResult("Ürün ve diğer varlıklar silindi");
        }

        [TransactionScopeAspect]
        public IResult UpdateProductWithOtherEntities(Product product,
            List<Barcode> barcodes,
            List<ProductPrice> productPrices,
            List<ProductCategory> productCategories)
        {
            _productDal.Update(product);

            if (productPrices != null && productPrices.Any())
            {
                productPrices.ForEach(p => p.ProductId = product.Id);
                _productPriceService.BulkAddOrUpdate(productPrices);
            }

            if (productCategories != null && productCategories.Any())
            {
                productCategories.ForEach(p => p.ProductId = product.Id);
                _productCategoryService.BulkAddOrUpdate(productCategories);
            }

            if (barcodes != null && barcodes.Any())
            {
                barcodes.ForEach(p => p.ProductId = product.Id);
                _barcodeService.BulkAddOrUpdate(barcodes);
            }

            return new SuccessResult("Ürün ve diğer varlıklar güncellendi");
        }
    }
}
