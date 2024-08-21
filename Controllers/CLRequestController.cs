using FamilyTree.BL.Services;
using FamilyTree.Core;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTree.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CLRequestController : ControllerBase
    {
        Response objResponse = new Response();

        private readonly IRequestService _requestService;
        public CLRequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_requestService.GetAll());
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            return Ok(_requestService.EntityExists(id));
        }


        [HttpPost]
        public IActionResult Create(Request entity)
        {
            _requestService.EntryType = enmEntryType.A;
            objResponse = _requestService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                objResponse = _requestService.PreSave(entity);
                objResponse = _requestService.AddOrUpdate(entity);
            }
            return Ok(_requestService.AddOrUpdate(entity));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Request entity)
        {
            _requestService.EntryType = enmEntryType.E;
            return Ok(_requestService.AddOrUpdate(entity));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _requestService.EntryType = enmEntryType.D;
            return Ok(_requestService.Delete(id));
        }
    }
}

