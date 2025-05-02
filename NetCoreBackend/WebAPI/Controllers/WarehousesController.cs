
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IContactService _contactService;

        public WarehousesController(IWarehouseService warehouseService, IContactService contactService)
        {
            _warehouseService = warehouseService;
            _contactService = contactService;
        }


        [HttpGet]
        public IActionResult GetListInclude()
        {
            var result = _warehouseService.GetListInclude();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{Id:int}")] //id/1
        public IActionResult GetById(int Id)
        {
            var result = _warehouseService.GetByIdInclude(Id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add(WarehouseAddressModel model)
        {
            var result = _warehouseService.AddWarehouseWithAddresses(model.Warehouse, model.Addresses);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{Id:int}")] //id/1
        public IActionResult Delete(int id)
        {
            var result = _warehouseService.DeleteWarehouseWithAddresses(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update(WarehouseAddressModel model)
        {
            var result = _warehouseService.UpdateWarehouseWithAddresses(model.Warehouse, model.Addresses);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        //** Contacts Methods **//

        [HttpGet("{warehouseId:int}/contacts")] //id/1/contacts
        public IActionResult GetContactsByWarehouseId(int warehouseId)
        {
            var result = _warehouseService.GetListContactsByWarehouseId(warehouseId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("contacts/{contactId:int}")] //id/1/contacts/1
        public IActionResult GetContactByContactId(int contactId)
        {
            var result = _contactService.GetByIdInclude(contactId);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPost("{warehouseId:int}/contacts")] //id/1/contacts
        public IActionResult AddContact(int warehouseId, [FromBody] WarehouseContactModel model)
        {
            var result = _contactService.AddWithContactDetailAndWarehouse(warehouseId, model.Contact, model.ContactDetails);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{warehouseId:int}/contacts/{contactId:int}")] //id/1/contacts/1
        public IActionResult DeleteContact(int warehouseId, int contactId)
        {
            var result = _contactService.DeleteWithContactDetailAndWarehouse(warehouseId, contactId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("contacts/{contactId:int}")] //id/1/contacts
        public IActionResult UpdateContact(int contactId, [FromBody] WarehouseContactModel model)
        {
            model.Contact.Id = contactId;
            var result = _contactService.UpdateWithContactDetailAndWarehouse(model.Contact, model.ContactDetails);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        //NOTE: Contact Details Methods **//
        [HttpDelete("contacts/{detailId}")]
        public IActionResult DeleteContactDetail(int detailId)
        {
            var result = _contactService.DeleteContactDetail(new ContactDetail { Id = detailId });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("contacts/details")]
        public IActionResult UpdateContactDetail(ContactDetail contactDetail)
        {
            var result = _contactService.UpdateContactDetail(contactDetail);
            return result.Success ? Ok(result) : BadRequest(result);
        }


    }
}