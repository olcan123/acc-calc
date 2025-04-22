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
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService) => _companyService = companyService;

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _companyService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("include")]
        public IActionResult GetAllWithInclude()
        {
            var result = _companyService.GetAllWithInclude();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            var result = _companyService.Get(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("include/id/{id}")]
        public IActionResult GetWithInclude(int id)
        {
            var result = _companyService.GetWithInclude(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add(Company company)
        {
            var result = _companyService.Add(company);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _companyService.Delete(new Company { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update(Company company)
        {
            var result = _companyService.Update(company);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}