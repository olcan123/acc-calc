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
    public class SalesController : ControllerBase
    {
        private readonly ISaleInvoiceService _saleInvoiceService;

        public SalesController(ISaleInvoiceService saleInvoiceService)
        {
            _saleInvoiceService = saleInvoiceService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _saleInvoiceService.GetListWithIncludes();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _saleInvoiceService.GetByIdWithDetails(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] SaleModel model)
        {
            var result = _saleInvoiceService.AddInvoice(model.Ledger, model.SaleInvoices[0], model.SaleInvoiceLines);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _saleInvoiceService.DeleteInvoice(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody] SaleModel model)
        {
            var result = _saleInvoiceService.UpdateInvoice(model.Ledger, model.SaleInvoices[0], model.SaleInvoiceLines);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
