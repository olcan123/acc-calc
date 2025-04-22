using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrate.EntityFramework.Context;
using Entities.Concrate;

namespace DataAccess.Concrate.Dal
{
    public class EfBankAccountDal : EfEntityRepositoryBase<BankAccount, FcdAccContext>, IBankAccountDal
    {
        public void AddWithCompany(int companyId, BankAccount bankAccount)
        {
            using var context = new FcdAccContext();
            var company = new Company() { Id = companyId };
            context.Companies.Attach(company);

            bankAccount.Companies.Add(company);

            context.BankAccounts.Add(bankAccount);
            context.SaveChanges();
        }
    }
}
