using FamilyTree.BL.Services;
using FamilyTree.Core;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTree.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CLRelationTypeController : ControllerBase
    {
        Response objResponse = new Response();

        private readonly IRelationTypeService _relationTypeService;
        public CLRelationTypeController(IRelationTypeService relationTypeService)
        {
            _relationTypeService = relationTypeService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_relationTypeService.GetAll());
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            return Ok(_relationTypeService.EntityExists(id));
        }

        [HttpPost]
        public IActionResult Create(RelationType entity)
        {
            _relationTypeService.EntryType = enmEntryType.A;
            objResponse = _relationTypeService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                objResponse = _relationTypeService.AddOrUpdate(entity);
            }
            //else
            //{
            //    objResponse = _relationTypeService.
            //}
            return Ok(_relationTypeService.AddOrUpdate(entity));
        }

        [HttpPut]
        public async Task<IActionResult> Update(RelationType entity)
        {
            _relationTypeService.EntryType = enmEntryType.E;
            return Ok(_relationTypeService.AddOrUpdate(entity));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _relationTypeService.EntryType = enmEntryType.D;
            return Ok(_relationTypeService.Delete(id));
        }
    }
}

