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
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseInvoiceService _purchaseInvoiceService;

        public PurchasesController(IPurchaseInvoiceService purchaseInvoiceService)
        {
            _purchaseInvoiceService = purchaseInvoiceService;
        }

        [HttpPost]
        public IActionResult Add([FromBody] PurchaseModel model)
        {
            var result = _purchaseInvoiceService.AddInvoice(model.Ledger, model.PurchaseInvoices[0], model.PurchaseInvoiceLines, model.PurchaseInvoiceExpenses);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}