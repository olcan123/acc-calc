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

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _contactDetailService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            var result = _contactDetailService.Get(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add(ContactDetail detail)
        {
            var result = _contactDetailService.Add(detail);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update(ContactDetail detail)
        {
            var result = _contactDetailService.Update(detail);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _contactDetailService.Delete(new ContactDetail { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
