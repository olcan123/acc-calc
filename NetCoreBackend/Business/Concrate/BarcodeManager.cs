using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using EFCore.BulkExtensions;
using Entities.Concrate;

namespace Business.Concrate
{
    public class BarcodeManager : IBarcodeService
    {
        private readonly IBarcodeDal _barcodeDal;

        public BarcodeManager(IBarcodeDal barcodeDal)
        {
            _barcodeDal = barcodeDal;
        }

        public IDataResult<List<Barcode>> GetList()
        {
            var result = _barcodeDal.GetAll();
            return new SuccessDataResult<List<Barcode>>(result);
        }

        public IDataResult<List<Barcode>> GetListByProductId(int productId)
        {
            var result = _barcodeDal.GetAll(b => b.ProductId == productId);
            return new SuccessDataResult<List<Barcode>>(result);
        }

        public IDataResult<Barcode> GetById(int id)
        {
            var result = _barcodeDal.Get(x => x.Id == id);
            return new SuccessDataResult<Barcode>(result);
        }

        public IDataResult<Barcode> GetByCode(string code)
        {
            var result = _barcodeDal.Get(x => x.Code == code);
            return new SuccessDataResult<Barcode>(result);
        }

        [ValidationAspect(typeof(BarcodeValidator), Priority = 1)]
        public IResult Add(Barcode barcode)
        {
            _barcodeDal.Add(barcode);
            return new SuccessResult("Barkod Eklendi");
        }

        public IResult Delete(Barcode barcode)
        {
            _barcodeDal.Delete(barcode);
            return new SuccessResult("Barkod Silindi");
        }

        [ValidationAspect(typeof(BarcodeValidator), Priority = 1)]
        public IResult Update(Barcode barcode)
        {
            _barcodeDal.Update(barcode);
            return new SuccessResult("Barkod Güncellendi");
        }

        //SECTION - BULK OPERATIONS
        [ValidationAspect(typeof(BarcodeValidator), Priority = 1)]
        public IResult BulkAdd(List<Barcode> barcodes)
        {
            _barcodeDal.BulkAdd(barcodes);
            return new SuccessResult("Barkodlar Eklendi");
        }

        public IResult BulkDelete(List<Barcode> barcodes)
        {
            _barcodeDal.BulkDelete(barcodes);
            return new SuccessResult("Barkodlar Silindi");
        }

        [ValidationAspect(typeof(BarcodeValidator), Priority = 1)]
        public IResult BulkAddOrUpdate(List<Barcode> barcodes)
        {
            _barcodeDal.MergeLinq(barcodes, (source, target) => source.Id == target.Id);
            return new SuccessResult("Barkodlar Eklendi/Güncellendi");
        }
    }
}
