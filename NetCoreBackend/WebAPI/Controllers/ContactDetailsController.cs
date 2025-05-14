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
    public class ContactDetailsController : ControllerBase
    {
        private readonly IContactDetailService _contactDetailService;

        public ContactDetailsController(IContactDetailService contactDetailService)
        {
            _contactDetailService = contactDetailService;
        }


        // //NOTE: Contact Details Methods **//
        [HttpDelete("id/{id:int}")]
        public IActionResult DeleteContactDetail(int id)
        {
            var result = _contactDetailService.Delete(new ContactDetail { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult UpdateContactDetail(ContactDetail contactDetail)
        {
            var result = _contactDetailService.Update(contactDetail);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}