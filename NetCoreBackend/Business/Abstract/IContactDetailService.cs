using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IContactDetailService
    {
        IDataResult<List<ContactDetail>> GetList();
        IDataResult<List<ContactDetail>> GetListByContactId(int contactId);
        IDataResult<ContactDetail> GetById(int id);
        IResult Add(ContactDetail contactDetail);
        IResult Delete(ContactDetail contactDetail);
        IResult Update(ContactDetail contactDetail);

        //Bulk
        IResult AddBulk(List<ContactDetail> contactDetails);
        IResult DeleteBulk(List<ContactDetail> contactDetails);
        IResult UpdateBulk(List<ContactDetail> contactDetails);
        IResult AddOrUpdateBulk(List<ContactDetail> contactDetails);
    }
}