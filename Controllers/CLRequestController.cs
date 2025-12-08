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

        [HttpPost("GetAll")]
        [Authorize("Member")]
        public IActionResult GetAll([FromBody] CommonSearchModel model)
        {
            SetUserContext();
            return Ok(_requestService.GetAll(model));
        }

        [HttpGet("GetById")]
        [Authorize("Member")]
        public IActionResult GetById(int id)
        {
            SetUserContext();
            return Ok(_requestService.GetById(id));
        }


        [HttpPost]
        [Authorize("Member")]
        public IActionResult Create([FromBody] DTORequest entity)
        {
            SetUserContext();
            _requestService.EntryType = enmEntryType.A;
            objResponse = _requestService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                _requestService.Presave(entity);
                objResponse = _requestService.AddOrUpdate();
            }
            return Ok(objResponse);
        }

        [HttpPut]
        [Authorize("Member")]
        public async Task<IActionResult> Update(DTORequest entity)
        {
            SetUserContext();
            _requestService.EntryType = enmEntryType.E;
            objResponse = _requestService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                _requestService.Presave(entity);
                objResponse = _requestService.AddOrUpdate();
            }
            return Ok(_requestService.AddOrUpdate());
        }

        [HttpPost("UpdateStatus")]
        public IActionResult UpdateStatus(int requestId, enmApprovalStatus ApprovalStatus = enmApprovalStatus.A)
        {
            SetUserContext();
            _requestService.ApprovalStatus = ApprovalStatus;
            objResponse = _requestService.PreApproveRequest(requestId);
            if (!objResponse.IsError)
            {
                objResponse = _requestService.ApproveRequest();
            }
            return Ok(objResponse);
        }

        [HttpDelete]
        [Authorize("Member")]
        public IActionResult Delete(int id)
        {
            SetUserContext();
            _requestService.EntryType = enmEntryType.D;
            return Ok(_requestService.Delete(id));
        }

        private void SetUserContext()
        {
            if (HttpContext.Items.TryGetValue("UserId", out object? userIdObj) && userIdObj is int userId)
            {
                _requestService.CurrentUserId = userId;
            }
            if (HttpContext.Items.TryGetValue("RoleId", out object? roleIdObj) && roleIdObj is int roleId)
            {
                _requestService.CurrentUserRole = roleId;
            }
        }
    }
}

