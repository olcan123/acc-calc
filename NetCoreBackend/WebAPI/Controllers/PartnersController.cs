
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerService _partnerService;

        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _partnerService.GetListInclude();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")] // for update
        public IActionResult GetById(int id)
        {
            var result = _partnerService.GetById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("details/id/{id}")]
        public IActionResult GetDetailsById(int id)
        {
            var result = _partnerService.GetByIdInclude(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] PartnerWithAddressModel model)
        {
            var result = _partnerService.AddWithAddress(model.Partner, model.Address);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update(PartnerWithAddressModel model)
        {
            var result = _partnerService.UpdateWithAddress(model.Partner, model.Address);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{partnerId:int}/addresses/{addressId:int}")]
        public IActionResult Delete(int partnerId, int addressId)
        {
            var result = _partnerService.DeleteWithAddress(addressId, partnerId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
