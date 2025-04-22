using Core.Utilities.Results;
using Entities.Concrate;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IContactDetailService
    {
        IDataResult<List<ContactDetail>> GetAll();
        IDataResult<ContactDetail> Get(int id);
        IResult Add(ContactDetail detail);
        IResult Update(ContactDetail detail);
        IResult Delete(ContactDetail detail);
    }
}
