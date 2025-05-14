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
    public class BarcodesController : ControllerBase
    {
        private readonly IBarcodeService _barcodeService;

        public BarcodesController(IBarcodeService barcodeService)
        {
            _barcodeService = barcodeService;
        }


        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _barcodeService.Delete(new Barcode { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}