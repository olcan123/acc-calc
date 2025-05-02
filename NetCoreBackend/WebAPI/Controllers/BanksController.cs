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
    public class BanksController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BanksController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bankService.GetList();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{Id:int}")] //id/1
        public IActionResult GetById(int Id)
        {
            var result = _bankService.GetById(Id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add(Bank bank)
        {
            var result = _bankService.Add(bank);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{Id:int}")] //id/1
        public IActionResult Delete(int Id)
        {
            var result = _bankService.Delete(new Bank { Id = Id });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update(Bank bank)
        {
            var result = _bankService.Update(bank);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}