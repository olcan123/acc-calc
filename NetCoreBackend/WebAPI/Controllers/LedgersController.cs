using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LedgersController : ControllerBase
    {
        private readonly ILedgerService _ledgerService;
        public LedgersController(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _ledgerService.GetListWithEntries();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _ledgerService.GetByIdWithEntries(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] LedgerModel model)
        {
            var result = _ledgerService.AddLedgerization(model.Ledger, model.LedgerEntries);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _ledgerService.DeleteLedgerization(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody] LedgerModel model)
        {
            var result = _ledgerService.UpdateLedgerization(model.Ledger, model.LedgerEntries);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}