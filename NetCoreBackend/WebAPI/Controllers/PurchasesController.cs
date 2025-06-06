using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseInvoiceService _purchaseInvoiceService;
        private readonly IPurchaseInvoiceLineService _purchaseInvoiceLineService;
        private readonly IPurchaseInvoiceExpenseService _purchaseInvoiceExpenseService;

        public PurchasesController(
            IPurchaseInvoiceService purchaseInvoiceService,
            IPurchaseInvoiceLineService purchaseInvoiceLineService,
            IPurchaseInvoiceExpenseService purchaseInvoiceExpenseService)
        {
            _purchaseInvoiceService = purchaseInvoiceService;
            _purchaseInvoiceLineService = purchaseInvoiceLineService;
            _purchaseInvoiceExpenseService = purchaseInvoiceExpenseService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _purchaseInvoiceService.GetListWithIncludes();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _purchaseInvoiceService.GetByIdWithDetails(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] PurchaseModel model)
        {
            var result = _purchaseInvoiceService.AddInvoice(model.Ledger, model.PurchaseInvoices[0], model.PurchaseInvoiceLines, model.PurchaseInvoiceExpenses);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _purchaseInvoiceService.DeleteInvoice(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }        [HttpPut]
        public IActionResult Update([FromBody] PurchaseModel model)
        {
            var result = _purchaseInvoiceService.UpdateInvoice(model.Ledger, model.PurchaseInvoices[0], model.PurchaseInvoiceLines, model.PurchaseInvoiceExpenses);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}/lines")]
        public IActionResult GetPurchaseLines(int id)
        {
            var result = _purchaseInvoiceLineService.GetListByInvoiceId(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}/expenses")]
        public IActionResult GetPurchaseExpenses(int id)
        {
            var result = _purchaseInvoiceExpenseService.GetListByInvoiceId(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}