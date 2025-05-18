using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        // SECTION - ASYNC METHODS
        public async Task<IDataResult<List<Category>>> GetListAsync()
        {
            var result = await _categoryDal.GetAllAsync();
            return new SuccessDataResult<List<Category>>(result);
        }

        // SECTION - SYNC METHODS

        public IDataResult<List<Category>> GetList()
        {
            var result = _categoryDal.GetAll();
            return new SuccessDataResult<List<Category>>(result);
        }

        public IDataResult<Category> GetById(int id)
        {
            var result = _categoryDal.Get(x => x.Id == id);
            return new SuccessDataResult<Category>(result);
        }

        [ValidationAspect(typeof(CategoryValidator), Priority = 1)]
        public IResult Add(Category category)
        {
            _categoryDal.Add(category);
            return new SuccessResult("Kategori Eklendi");
        }

        public IResult Delete(Category category)
        {
            _categoryDal.Delete(category);
            return new SuccessResult("Kategori Silindi");
        }

        [ValidationAspect(typeof(CategoryValidator), Priority = 1)]
        public IResult Update(Category category)
        {
            _categoryDal.Update(category);
            return new SuccessResult("Kategori Güncellendi");
        }

        //SECTION - BULK OPERATIONS
        [ValidationAspect(typeof(CategoryValidator), Priority = 1)]
        public IResult BulkAdd(List<Category> categories)
        {
            _categoryDal.BulkAdd(categories);
            return new SuccessResult("Kategoriler Eklendi");
        }

        public IResult BulkDelete(List<Category> categories)
        {
            _categoryDal.BulkDelete(categories);
            return new SuccessResult("Kategoriler Silindi");
        }

        [ValidationAspect(typeof(CategoryValidator), Priority = 1)]
        public IResult BulkAddOrUpdate(List<Category> categories)
        {
            _categoryDal.BulkAddOrUpdate(categories);
            return new SuccessResult("Kategoriler Eklendi/Güncellendi");
        }
    }
}
