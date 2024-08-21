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

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_personService.GetAll());
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            return Ok(_personService.EntityExists(id));
        }

        [HttpGet("GetWholeTree")]
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
                objResponse = _personService.AddOrUpdate(entity);
            }
            return Ok(_personService.AddOrUpdate(entity));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Person entity)
        {
            _personService.EntryType = enmEntryType.E;
            return Ok(_personService.AddOrUpdate(entity));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _personService.EntryType = enmEntryType.D;
            return Ok(_personService.Delete(id));
        }
    }
}

