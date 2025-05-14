using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/warehouses/{warehouseId}/contacts")]
    public class ContactWarehousesController : ControllerBase
    {
        private readonly IContactWarehouseService _contactWarehouseService;

        public ContactWarehousesController(IContactWarehouseService contactWarehouseService)
        {
            _contactWarehouseService = contactWarehouseService;
        }

        [HttpGet]
        public IActionResult GetContactsByWarehouseId(int warehouseId)
        {
            var result = _contactWarehouseService.GetListContactsByWarehouseId(warehouseId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("~/api/warehouses/contacts/{contactId:int}")]
        public IActionResult GetContactByContactId(int contactId)
        {
            var result = _contactWarehouseService.GetByIdInclude(contactId);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPost]
        public IActionResult AddContact(int warehouseId, [FromBody] WarehouseContactModel model)
        {
            var result = _contactWarehouseService.AddWithContactDetailAndWarehouse(warehouseId, model.Contact, model.ContactDetails);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("~/api/warehouses/contacts/{contactId:int}")]
        public IActionResult UpdateContact(int contactId, [FromBody] WarehouseContactModel model)
        {
            model.Contact.Id = contactId;
            var result = _contactWarehouseService.UpdateWithContactDetailAndWarehouse(model.Contact, model.ContactDetails);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{contactId:int}")]
        public IActionResult DeleteContact(int warehouseId, int contactId)
        {
            var result = _contactWarehouseService.DeleteWithContactDetailAndWarehouse(warehouseId, contactId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
