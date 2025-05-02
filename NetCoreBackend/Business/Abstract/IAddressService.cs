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
        public IDataResult<List<Address>> GetList();
        public IDataResult<Address> GetById(int id);
        public IResult Add(Address address);
        public IResult AddBulk(List<Address> addresses);
        public IResult Delete(Address address);
        public IResult DeleteBulk(List<Address> addresses);
        public IResult Update(Address address);
        public IResult UpdateBulk(List<Address> addresses);
    }
}