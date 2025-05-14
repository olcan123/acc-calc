using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class VatManager : IVatService
    {
        private readonly IVatDal _vatDal;

        public VatManager(IVatDal vatDal)
        {
            _vatDal = vatDal;
        }
        public IDataResult<List<Vat>> GetList()
        {
            var result = _vatDal.GetAll();
            return new SuccessDataResult<List<Vat>>(result);
        }

        public IDataResult<Vat> GetById(int id)
        {
            var result = _vatDal.Get(x => x.Id == id);
            return new SuccessDataResult<Vat>(result);
        }


        [ValidationAspect(typeof(VatValidator), Priority = 1)]
        public IResult Add(Vat vat)
        {
            _vatDal.Add(vat);
            return new SuccessResult("KDV Oranı Eklendi");
        }


        [ValidationAspect(typeof(VatValidator), Priority = 1)]
        public IResult Update(Vat vat)
        {
            _vatDal.Update(vat);
            return new SuccessResult("KDV Oranı Güncellendi");
        }

        public IResult Delete(Vat vat)
        {
            _vatDal.Delete(vat);
            return new SuccessResult("KDV Oranı Silindi");
        }
    }
}