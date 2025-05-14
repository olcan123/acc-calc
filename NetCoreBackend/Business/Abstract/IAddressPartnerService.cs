using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IAddressPartnerService
    {
        public IResult Add(int partnerId, Address address);
        public IResult Delete(int addressId, int partnerId);
        public IResult Update(Address address);
    }
}