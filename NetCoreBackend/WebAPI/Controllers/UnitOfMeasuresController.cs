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
    public class UnitOfMeasuresController : ControllerBase
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public UnitOfMeasuresController(IUnitOfMeasureService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _unitOfMeasureService.GetList();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _unitOfMeasureService.GetById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UnitOfMeasure unitOfMeasure)
        {
            var result = _unitOfMeasureService.Add(unitOfMeasure);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UnitOfMeasure unitOfMeasure)
        {
            var result = _unitOfMeasureService.Update(unitOfMeasure);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _unitOfMeasureService.Delete(new UnitOfMeasure { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
