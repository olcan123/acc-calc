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
using EFCore.BulkExtensions;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace Business.Concrate
{
    public class CompanyManager : ICompanyService
    {
        private readonly ICompanyDal _companyDal;
        private readonly IWarehouseService _warehouseService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IBankAccountCompanyService _bankAccountCompanyService;

        public CompanyManager(ICompanyDal companyDal, IWarehouseService warehouseService, IBankAccountService bankAccountService, IBankAccountCompanyService bankAccountCompanyService)
        {
            _companyDal = companyDal;
            _warehouseService = warehouseService;
            _bankAccountService = bankAccountService;
            _bankAccountCompanyService = bankAccountCompanyService;
        }

        public IDataResult<Company> GetById(int id)
        {
            var result = _companyDal.Get(c => c.Id == id);
            return new SuccessDataResult<Company>(result);
        }

        public IDataResult<Company> GetByIdInclude(int id)
        {
            var result = _companyDal.GetWithIncludeChain(x => x.Include(x => x.BankAccountCompanies).ThenInclude(x => x.BankAccount), x => x.Id == id);
            return new SuccessDataResult<Company>(result);
        }

        public IDataResult<List<Company>> GetList()
        {
            var result = _companyDal.GetAll();
            return new SuccessDataResult<List<Company>>(result);
        }
        

        public IDataResult<List<Company>> GetListInclude()
        {
            var result = _companyDal.GetAllWithIncludeChain(x => x.Include(x => x.BankAccountCompanies).ThenInclude(x => x.BankAccount));
            return new SuccessDataResult<List<Company>>(result);
        }

        [ValidationAspect(typeof(CompanyValidator), Priority = 1)]
        public IResult Add(Company company)
        {
            _companyDal.Add(company);
            return new SuccessResult("Sirket Eklendi");
        }

        public IResult Delete(Company company)
        {
            _companyDal.Delete(company);
            return new SuccessResult("Sirket Silindi");
        }

        [ValidationAspect(typeof(CompanyValidator), Priority = 1)]
        public IResult Update(Company company)
        {
            _companyDal.Update(company);
            return new SuccessResult("Sirket Güncellendi");
        }
    }
}