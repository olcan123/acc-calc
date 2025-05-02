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
    [Route("api/companies/{companyId}/bankaccounts")]
    public class BankAccountCompaniesController : ControllerBase
    {
        private readonly IBankAccountCompanyService _bankAccountCompanyService;

        public BankAccountCompaniesController(IBankAccountCompanyService bankAccountCompanyService)
        {
            _bankAccountCompanyService = bankAccountCompanyService;
        }

        [HttpGet]
        public IActionResult GetListByCompanyId(int companyId)
        {
            var result = _bankAccountCompanyService.GetListIncludeByCompanyId(companyId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{bankaccountId:int}")] //id/1
        public IActionResult GetByBankAccountId(int companyId, int bankaccountId)
        {
            var result = _bankAccountCompanyService.GetIncludeByBankAccountIdAndCompanyId(companyId, bankaccountId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add(int companyId, [FromBody] BankAccount bankAccount)
        {
            var result = _bankAccountCompanyService.AddBankAccountCompany(companyId, bankAccount);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{bankaccountId:int}")] //id/1
        public IActionResult Delete(int companyId, int bankaccountId)
        {
            var result = _bankAccountCompanyService.DeleteBankAccountCompany(companyId, bankaccountId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{bankaccountId:int}")] //id/1
        public IActionResult Update(int companyId, int bankaccountId, [FromBody]BankAccount bankAccount)
        {
            bankAccount.Id = bankaccountId;
            var result = _bankAccountCompanyService.UpdateBankAccountCompany(companyId, bankAccount);
            return result.Success ? Ok(result) : BadRequest(result);
        }


    }
}