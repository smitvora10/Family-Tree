using FamilyTree.BL.Services;
using FamilyTree.Core;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FamilyTree.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CLUserController : ControllerBase
    {
        Response objResponse = new Response();

        private readonly IUserService _userService;
        public CLUserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("GetAll")]
        [Authorize("Admin")]
        public IActionResult GetAll([FromBody] CommonSearchModel model)
        {
            return Ok(_userService.GetAll(model));
        }

        [HttpPost("GetDDLData")]
        [Authorize("Admin")]
        public IActionResult GetDDLData([FromBody] CommonDDLRequest model)
        {
            return Ok(_userService.GetDDLData(model));
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            return Ok(_userService.GetById(id));
        }

        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Create(User entity)
        {
            _userService.EntryType = enmEntryType.A;
            objResponse = _userService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                _userService.Presave(entity);
                objResponse = _userService.AddOrUpdate();
            }
            return Ok(_userService.AddOrUpdate());
        }

        [HttpPut]
        [Authorize("Admin")]
        public async Task<IActionResult> Update(User entity)
        {
            _userService.EntryType = enmEntryType.E;
            objResponse = _userService.ValidationBeforePreSave(entity);
            if (!objResponse.IsError)
            {
                _userService.Presave(entity);
                objResponse = _userService.AddOrUpdate();
            }
            return Ok(_userService.AddOrUpdate());
        }

        [HttpDelete]
        [Authorize("Admin")]
        public IActionResult Delete(int id)
        {
            _userService.EntryType = enmEntryType.D;
            return Ok(_userService.Delete(id));
        }
    }
}

