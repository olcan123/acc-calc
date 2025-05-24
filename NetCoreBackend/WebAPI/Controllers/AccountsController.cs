using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _accountService.GetList();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("async")]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _accountService.GetListAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _accountService.GetById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("parent/{parentId}")]
        public IActionResult GetByParentId(int parentId)
        {
            var result = _accountService.GetByParentId(parentId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Account account)
        {
            var result = _accountService.Add(account);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Account account)
        {
            var result = _accountService.Update(account);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var account = _accountService.GetById(id);
            if (!account.Success)
            {
                return NotFound(new { Success = false, Message = "Hesap bulunamadı" });
            }

            var result = _accountService.Delete(account.Data);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
