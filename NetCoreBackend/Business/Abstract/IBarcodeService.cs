using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IBarcodeService
    {
        IDataResult<List<Barcode>> GetList();
        IDataResult<List<Barcode>> GetListByProductId(int productId);
        IDataResult<Barcode> GetById(int id);
        IDataResult<Barcode> GetByCode(string code);
        IResult Add(Barcode barcode);
        IResult Update(Barcode barcode);
        IResult Delete(Barcode barcode);

        //SECTION - BULK OPERATIONS
        IResult BulkAdd(List<Barcode> barcodes);
        IResult BulkDelete(List<Barcode> barcodes);
        IResult BulkAddOrUpdate(List<Barcode> barcodes);
    }
}
