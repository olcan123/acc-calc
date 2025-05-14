
using Core.Utilities.Results;
using Entities.Concrate;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IPartnerService
    {
        IDataResult<List<Partner>> GetList();
        IDataResult<List<Partner>> GetListInclude();
        IDataResult<Partner> GetById(int id);
        IDataResult<Partner> GetByIdInclude(int id);
        IDataResult<Partner> GetListContactsByPartnerId(int partnerId);
        IResult Add(Partner partner);
        IResult Update(Partner partner);
        IResult Delete(Partner partner);

        //SECTION - Address Partner
        IResult AddWithAddress(Partner partner, Address address);
        IResult UpdateWithAddress(Partner partner, Address address);
        IResult DeleteWithAddress(int addressId, int partnerId);
    }
}
