
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/partners/{partnerId}/bankaccounts")]
    public class BankAccountPartnersController : ControllerBase
    {
        private readonly IBankAccountPartnerService _bankAccountPartnerService;

        public BankAccountPartnersController(IBankAccountPartnerService bankAccountPartnerService)
        {
            _bankAccountPartnerService = bankAccountPartnerService;
        }

        [HttpGet]
        public IActionResult GetListByPartnerId(int partnerId)
        {
            var result = _bankAccountPartnerService.GetListIncludeByPartnerId(partnerId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{bankaccountId:int}")]
        public IActionResult GetByBankAccountId(int bankaccountId)
        {
            var result = _bankAccountPartnerService.GetByBankAccountId(bankaccountId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add(int partnerId, [FromBody] BankAccount bankAccount)
        {
            var result = _bankAccountPartnerService.AddBankAccountPartner(partnerId, bankAccount);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{bankaccountId:int}")]
        public IActionResult Delete(int partnerId, int bankaccountId)
        {
            var result = _bankAccountPartnerService.DeleteBankAccountPartner(partnerId, bankaccountId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{bankaccountId:int}")]
        public IActionResult Update(int partnerId, int bankaccountId, [FromBody] BankAccount bankAccount)
        {
            bankAccount.Id = bankaccountId;
            var result = _bankAccountPartnerService.UpdateBankAccountPartner(partnerId, bankAccount);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
