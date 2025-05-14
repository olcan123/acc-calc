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
    public class UnitOfMeasureManager : IUnitOfMeasureService
    {
        private readonly IUnitOfMeasureDal _unitOfMeasureDal;

        public UnitOfMeasureManager(IUnitOfMeasureDal unitOfMeasureDal)
        {
            _unitOfMeasureDal = unitOfMeasureDal;
        }        public IDataResult<List<UnitOfMeasure>> GetList()
        {
            var result = _unitOfMeasureDal.GetAll();
            return new SuccessDataResult<List<UnitOfMeasure>>(result);
        }

        public IDataResult<UnitOfMeasure> GetById(int id)
        {
            var result = _unitOfMeasureDal.Get(x => x.Id == id);
            return new SuccessDataResult<UnitOfMeasure>(result);
        }


        [ValidationAspect(typeof(UnitOfMeasureValidator), Priority = 1)]
        public IResult Add(UnitOfMeasure unitOfMeasure)
        {
            _unitOfMeasureDal.Add(unitOfMeasure);
            return new SuccessResult("Birim Eklendi");
        }

        [ValidationAspect(typeof(UnitOfMeasureValidator), Priority = 1)]
        public IResult Update(UnitOfMeasure unitOfMeasure)
        {
            _unitOfMeasureDal.Update(unitOfMeasure);
            return new SuccessResult("Birim Güncellendi");
        }

        public IResult Delete(UnitOfMeasure unitOfMeasure)
        {
            _unitOfMeasureDal.Delete(unitOfMeasure);
            return new SuccessResult("Birim Silindi");
        }
    }
}
