using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface ICompanyService
    {
        IDataResult<List<Company>> GetList();
        IDataResult<List<Company>> GetListInclude();
        IDataResult<Company> GetById(int id);
        IDataResult<Company> GetByIdInclude(int id);
        IResult Add(Company company);
        IResult Update(Company company);
        IResult Delete(Company company);
    }
}