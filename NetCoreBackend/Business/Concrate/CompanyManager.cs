using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class CompanyManager : ICompanyService
    {
        private readonly ICompanyDal _companyDal;
        private readonly IWarehouseService _warehouseService;
        private readonly IBankAccountService _bankAccountService;

        public CompanyManager(ICompanyDal companyDal, IWarehouseService warehouseService, IBankAccountService bankAccountService)
        {
            _companyDal = companyDal;
            _warehouseService = warehouseService;
            _bankAccountService = bankAccountService;
        }

        public IDataResult<List<Company>> GetAll()
        {
            var companies = _companyDal.GetAll();
            return new SuccessDataResult<List<Company>>(companies);
        }

        public IDataResult<List<Company>> GetAllWithInclude()
        {
            var companiesWithInclude = _companyDal.GetAllWithIncludeChain(query => query.Include(x => x.BankAccounts).Include(x => x.Warehouses).ThenInclude(x => x.Addresses));
            return new SuccessDataResult<List<Company>>(companiesWithInclude);
        }

        public IDataResult<Company> Get(int id)
        {
            var company = _companyDal.Get(x => x.Id == id);
            return new SuccessDataResult<Company>(company);
        }

        public IDataResult<Company> GetWithInclude(int id)
        {
            var companyWithInclude = _companyDal.GetWithIncludeChain(query => query.Include(x => x.BankAccounts).Include(x => x.Warehouses).ThenInclude(x => x.Addresses), x => x.Id == id);
            return new SuccessDataResult<Company>(companyWithInclude);
        }

        public IResult Add(Company company)
        {
            _companyDal.AddWithChildren(company);
            return new SuccessResult("Added");
        }

        [TransactionScopeAspect]
        public IResult Delete(Company company)
        {
            var result = _companyDal.GetWithIncludeChain(query => query.Include(x => x.BankAccounts).Include(x => x.Warehouses).ThenInclude(x => x.Addresses), x => x.Id == company.Id);
            var warehouses = result.Warehouses.ToList();
            for (int i = 0; i < warehouses.Count; i++)
                _warehouseService.Delete(warehouses[i]);

            var bankAccounts = result.BankAccounts.ToList();
            for (int i = 0; i < bankAccounts.Count; i++)
                _bankAccountService.Delete(bankAccounts[i]);

            _companyDal.Delete(company);
            return new SuccessResult("Deleted");
        }

        public IResult Update(Company company)
        {
            _companyDal.InsertOrUpdateWithChildren(company);
            return new SuccessResult("Updated");
        }
    }
}