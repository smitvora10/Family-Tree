using FamilyTree.BL.Services;
using FamilyTree.Core;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTree.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CLOccupationController : ControllerBase
    {
        Response objResponse = new Response();

        private readonly IOccupationService _occupationService;
        public CLOccupationController(IOccupationService occupationService)
        {
            _occupationService = occupationService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_occupationService.GetAll());
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            return Ok(_occupationService.EntityExists(id));
        }

        [HttpPost]
        public IActionResult Create(Occupation entity)
        {
            _occupationService.EntryType = enmEntryType.A;
            objResponse = _occupationService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                _occupationService.Presave(entity);  
                objResponse = _occupationService.AddOrUpdate();
            }
            //else
            //{
            //    objResponse = _occupationService.
            //}
            return Ok(objResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Occupation entity)
        {
            _occupationService.EntryType = enmEntryType.E;
            _occupationService.Presave(entity);
            return Ok(_occupationService.AddOrUpdate());
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _occupationService.EntryType = enmEntryType.D;
            return Ok(_occupationService.Delete(id));
        }
    }
}

