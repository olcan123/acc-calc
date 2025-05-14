using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _productService.GetListInclude();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetByIdInclude(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ProductOtherEntitiesModel model)
        {
            var result = _productService.AddProductWithOtherEntities(model.Product, model.Barcodes, model.ProductPrices, model.ProductCategories);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ProductOtherEntitiesModel model)
        {
            var result = _productService.UpdateProductWithOtherEntities(model.Product, model.Barcodes, model.ProductPrices, model.ProductCategories);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _productService.DeleteProductWithOtherEntities(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}