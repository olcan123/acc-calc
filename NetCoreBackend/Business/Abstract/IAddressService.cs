using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IAddressService
    {
        IDataResult<List<Address>> GetAll();
        IDataResult<Address> Get(int id);
        IResult Add(Address address);
        IResult Update(Address address);
        IResult Delete(Address address);
        IResult BulkDelete(List<Address> addresses);
    }

}