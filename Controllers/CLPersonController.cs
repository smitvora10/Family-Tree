using FamilyTree.BL.Services;
using FamilyTree.Core;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTree.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CLPersonController : ControllerBase
    {
        Response objResponse = new Response();

        private readonly IPersonService _personService;
        public CLPersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("GetAll")]
        [Authorize("Member")]
        public IActionResult GetAll([FromBody] CommonSearchModel model)
        {
            return Ok(_personService.GetAll(model));
        }

        [HttpGet("GetById")]
        [Authorize("Member")]
        public IActionResult GetById(int id)
        {
            return Ok(_personService.GetById(id));
        }

        [HttpGet("GetImage/{id}")]
        [Authorize("Member")]
        public IActionResult GetImage(int id)
        {
            return Ok(_personService.GetPersonImage(id));
        }

        [HttpPost("GetPersonDDL")]
        [Authorize("Member")]
        public IActionResult GetPersonDDL([FromBody] CommonSearchModel model)
        {
            return Ok(_personService.GetPersonDDL(model));
        }

        [HttpPost("GetDDLData")]
        [Authorize("Member")]
        public IActionResult GetDDLData([FromBody] CommonDDLRequest model)
        {
            return Ok(_personService.GetDDLData(model));
        }

        [HttpGet("GetWholeTree")]
        [Authorize("Member")]
        public IActionResult GetWholeTree()
        {
            return Ok(_personService.GetWholeTree());
        }

        [HttpPost]
        public IActionResult Create(Person entity)
        {
            _personService.EntryType = enmEntryType.A;
            objResponse = _personService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                _personService.Presave(entity);
                objResponse = _personService.AddOrUpdate();
            }
            return Ok(objResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Person entity)
        {
            _personService.EntryType = enmEntryType.E;
            objResponse = _personService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                _personService.Presave(entity);
                objResponse = _personService.AddOrUpdate();
            }
            return Ok(objResponse);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _personService.EntryType = enmEntryType.D;
            return Ok(_personService.Delete(id));
        }

        [HttpPost("UploadImage")]
        public IActionResult UploadImage([FromBody] UploadImageRequest request)
        {
            objResponse = _personService.UploadImage(request.PersonId, request.ImageBase64);
            return Ok(objResponse);
        }
    }

    public class UploadImageRequest
    {
        public int PersonId { get; set; }
        public string? ImageBase64 { get; set; }
    }
}

