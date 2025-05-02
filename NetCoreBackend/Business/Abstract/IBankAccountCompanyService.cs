using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IBankAccountCompanyService
    {
        //BankAccountCompany
        public IDataResult<List<BankAccountCompany>> GetList();
        public IDataResult<List<BankAccountCompany>> GetListIncludeByCompanyId(int companyId); //BankAccountCompany();
        public IDataResult<List<BankAccountCompany>> GetListIncludeByBankAccountId(int bankAccountId);
        public IDataResult<BankAccountCompany> GetIncludeByBankAccountIdAndCompanyId(int bankAccountId, int companyId);
        public IDataResult<BankAccountCompany> GetByBankAccountId(int bankAccountId);
        public IDataResult<BankAccountCompany> GetByCompanyId(int companyId);

        public IResult Add(BankAccountCompany bankAccountCompany);
        public IResult Update(BankAccountCompany bankAccountCompany);
        public IResult Delete(BankAccountCompany bankAccountCompany);

        //Bank + Company + BankAccount + BankAccountCompany
        public IResult AddBankAccountCompany(int companyId, BankAccount bankAccount);
        public IResult UpdateBankAccountCompany(int companyId, BankAccount bankAccount);
        public IResult DeleteBankAccountCompany(int companyId, int bankAccountId);


    }
}