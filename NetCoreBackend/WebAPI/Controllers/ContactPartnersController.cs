using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/partners/{partnerId}/contacts")]
    public class ContactPartnersController : ControllerBase
    {
        private readonly IContactPartnerService _contactPartnerService;

        public ContactPartnersController(IContactPartnerService contactPartnerService)
        {
            _contactPartnerService = contactPartnerService;
        }

        [HttpGet]
        public IActionResult GetContactsByPartnerId(int partnerId)
        {
            var result = _contactPartnerService.GetListContactsByPartnerId(partnerId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("~/api/partners/contacts/{contactId:int}")]
        public IActionResult GetContactByContactId(int contactId)
        {
            var result = _contactPartnerService.GetByIdInclude(contactId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult AddContact(int partnerId, [FromBody] PartnerContactModel model)
        {
            var result = _contactPartnerService.AddWithContactDetailAndPartner(partnerId, model.Contact, model.ContactDetails);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("~/api/partners/contacts/{contactId:int}")]
        public IActionResult UpdateContact(int contactId, [FromBody] PartnerContactModel model)
        {
            model.Contact.Id = contactId;
            var result = _contactPartnerService.UpdateWithContactDetailAndPartner(model.Contact, model.ContactDetails);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{contactId:int}")]
        public IActionResult DeleteContact(int partnerId, int contactId)
        {
            var result = _contactPartnerService.DeleteWithContactDetailAndPartner(partnerId, contactId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}