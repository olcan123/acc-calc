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
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _companyService.GetList();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var result = _companyService.GetById(Id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add(Company company)
        {
            var result = _companyService.Add(company);
            return result.Success ? Ok(result) : BadRequest(result);

        }

        [HttpDelete("id/{Id:int}")]
        public IActionResult Delete(int Id)
        {
            var result = _companyService.Delete(new Company { Id = Id });
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