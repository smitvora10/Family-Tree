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
                _requestService.Presave(entity);
                objResponse = _requestService.AddOrUpdate();
            }
            return Ok(_requestService.AddOrUpdate());
        }

        [HttpPut]
        public IActionResult ApproveRequest(int requestId, enmApprovalStatus ApprovalStatus = enmApprovalStatus.A)
        {
            _requestService.ApprovalStatus = ApprovalStatus;
            objResponse = _requestService. PreApproveRequest (requestId);
            if (!objResponse.IsError)
            {
                _requestService.Presave((Request)objResponse.DataModel);
                objResponse = _requestService.ApproveRequest();
            }
            return Ok(objResponse);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _requestService.EntryType = enmEntryType.D;
            return Ok(_requestService.Delete(id));
        }
    }
}

