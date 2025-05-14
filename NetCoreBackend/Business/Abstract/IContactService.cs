using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IContactService
    {
        IDataResult<List<Contact>> GetList();
        IDataResult<List<Contact>> GetListInclude();
        IDataResult<Contact> GetById(int id);
        IDataResult<Contact> GetByIdInclude(int id);

        IResult Add(Contact contact);
        IResult Update(Contact contact);
        IResult Delete(Contact contact);
    }
}