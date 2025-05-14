using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IBankAccountPartnerService
    {
        IDataResult<List<BankAccountPartner>> GetList();
        IDataResult<List<BankAccountPartner>> GetListIncludeByPartnerId(int partnerId);
        IDataResult<List<BankAccountPartner>> GetListIncludeByBankAccountId(int bankAccountId);
        IDataResult<BankAccountPartner> GetIncludeByBankAccountIdAndPartnerId(int bankAccountId, int partnerId);
        IDataResult<BankAccount> GetByBankAccountId(int bankAccountId);
        IDataResult<BankAccountPartner> GetByPartnerId(int partnerId);
        IResult Add(BankAccountPartner bankAccountPartner);
        IResult Update(BankAccountPartner bankAccountPartner);
        IResult Delete(BankAccountPartner bankAccountPartner);
        IResult AddBankAccountPartner(int partnerId, BankAccount bankAccount);
        IResult UpdateBankAccountPartner(int partnerId, BankAccount bankAccount);
        IResult DeleteBankAccountPartner(int partnerId, int bankAccountId);
    }
}
