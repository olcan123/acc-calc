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
    public class VatsController : ControllerBase
    {
        private readonly IVatService _vatService;

        public VatsController(IVatService vatService)
        {
            _vatService = vatService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _vatService.GetList();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _vatService.GetById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Vat vat)
        {
            var result = _vatService.Add(vat);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Vat vat)
        {
            var result = _vatService.Update(vat);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _vatService.Delete(new Vat { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}