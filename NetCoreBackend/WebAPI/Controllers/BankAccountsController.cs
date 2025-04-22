using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountsController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bankAccountService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            var result = _bankAccountService.Get(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("id/{id}")]
        public IActionResult Add(int id, [FromBody] BankAccount bankAccount)
        {
            var result = _bankAccountService.AddWithCompany(bankAccount, id, "Companies");
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update(BankAccount bankAccount)
        {
            var result = _bankAccountService.Update(bankAccount);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _bankAccountService.Delete(new BankAccount { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
