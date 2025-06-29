//using FamilyTree.BL.Services;
//using FamilyTree.Core;
//using FamilyTree.Models.Common;
//using FamilyTree.Models.Master;
//using Microsoft.AspNetCore.Mvc;

//namespace FamilyTree.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class CLQualificationController : ControllerBase
//    {
//        Response objResponse = new Response();

//        private readonly IQualificationService _qualificationService;
//        public CLQualificationController(IQualificationService qualificationService)
//        {
//            _qualificationService = qualificationService;
//        }

//        [HttpGet("GetAll")]
//        public IActionResult GetAll()
//        {
//            return Ok(_qualificationService.GetAll());
//        }

//        [HttpGet("GetById")]
//        public IActionResult GetById(int id)
//        {
//            return Ok(_qualificationService.EntityExists(id));
//        }

//        [HttpPost]
//        public IActionResult Create(Qualification entity)
//        {
//            _qualificationService.EntryType = enmEntryType.A;
//            objResponse = _qualificationService.ValidationBeforePreSave(entity);
//            if (!objResponse.IsError)
//            {
//                _qualificationService.Presave(entity);  
//                objResponse = _qualificationService.AddOrUpdate();
//            }
//            //else
//            //{
//            //    objResponse = _qualificationService.
//            //}
//            return Ok(objResponse);
//        }

//        [HttpPut]
//        public async Task<IActionResult> Update(Qualification entity)
//        {
//            _qualificationService.EntryType = enmEntryType.E;
//            _qualificationService.Presave(entity);
//            return Ok(_qualificationService.AddOrUpdate());
//        }

//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            _qualificationService.EntryType = enmEntryType.D;
//            return Ok(_qualificationService.Delete(id));
//        }
//    }
//}

