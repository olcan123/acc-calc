using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        
        [HttpDelete("productId/{productId}/categoryId/{categoryId}")]
        public IActionResult Delete(int productId, int categoryId)
        {
            var result = _productCategoryService.Delete(new ProductCategory { ProductId = productId, CategoryId = categoryId });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
