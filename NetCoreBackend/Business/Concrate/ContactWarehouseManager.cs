using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;

namespace Business.Concrate
{
    public class ContactWarehouseManager : IContactWarehouseService
    {
        private readonly IContactWarehouseDal _contactWarehouseDal;

        public ContactWarehouseManager(IContactWarehouseDal contactWarehouseDal)=>_contactWarehouseDal=contactWarehouseDal;

        public IResult Add(int warehouseId, int contactId)
        {
            _contactWarehouseDal.Add(new Entities.Concrate.ContactWarehouse { WarehouseId = warehouseId, ContactId = contactId });
            return new SuccessResult("İletişim Bilgisi Eklendi");
        }

        public IResult Delete(int warehouseId, int contactId)
        {
            _contactWarehouseDal.Delete(new Entities.Concrate.ContactWarehouse { WarehouseId = warehouseId, ContactId = contactId });
            return new SuccessResult("İletişim Bilgisi Silindi");
        }
    }
}