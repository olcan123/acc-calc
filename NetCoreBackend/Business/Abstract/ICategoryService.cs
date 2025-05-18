using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        //SECTION - ASYNC OPERATIONS
        Task<IDataResult<List<Category>>> GetListAsync();

        //SECTION - SYNC OPERATIONS
        IDataResult<List<Category>> GetList();
        IDataResult<Category> GetById(int id);
        IResult Add(Category category);
        IResult Update(Category category);
        IResult Delete(Category category);

        //SECTION - BULK OPERATIONS
        IResult BulkAdd(List<Category> categories);
        IResult BulkDelete(List<Category> categories);
        IResult BulkAddOrUpdate(List<Category> categories);
    }
}
