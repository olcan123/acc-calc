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
    public class ProductPricesController : ControllerBase
    {
        private readonly IProductPriceService _productPriceService;

        public ProductPricesController(IProductPriceService productPriceService)
        {
            _productPriceService = productPriceService;
        }



        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _productPriceService.Delete(new ProductPrice { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
