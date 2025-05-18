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
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("async")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _currencyService.GetListAsync();
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _currencyService.GetList();
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _currencyService.GetById(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Currency currency)
        {
            var result = _currencyService.Add(currency);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Currency currency)
        {
            var result = _currencyService.Update(currency);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _currencyService.Delete(new Currency { Id = id });
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}