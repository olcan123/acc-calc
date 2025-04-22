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
        public IDataResult<List<Company>> GetAll();
        public IDataResult<List<Company>> GetAllWithInclude();
        public IDataResult<Company> Get(int id);
        public IDataResult<Company> GetWithInclude(int id);
        public IResult Add(Company company);
        public IResult Update(Company company);
        public IResult Delete(Company company);
    }
}