using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using Request = FamilyTree.Models.Master.Request;

namespace FamilyTree.BL.Services
{
    public class BLRequest : BLCommon<Request>, IRequestService
    {
        private readonly IRequestRepository _dbContext;
        private readonly DbSet<Request> _dbSet;
        private readonly DataContext _context;
        private readonly IPersonService _personService;
        public Request objRequest { get; set; }
        public enmApprovalStatus ApprovalStatus { get; set; }

        public BLRequest(IRequestRepository dbContext, IPersonService personService, DataContext context) : base(dbContext)
        {
            _personService = personService;
            _dbContext = dbContext;
            _dbSet = context.Set<Request>();
        }

        public override Response GetAll()
        {
            List<Request> lstRequest = _dbContext.GetAll();
            return response;

        }
        public Response ApproveRequest()
        {
            Person objPerson = JsonConvert.DeserializeObject<Person>(objRequest.Person);
            _personService.EntryType = objRequest.Action;
            response = _personService.ValidationBeforePreSave(objPerson);
            if (!response.IsError)
            {
                response = _personService.AddOrUpdate();
                objRequest.ApprovalStatus = enmApprovalStatus.A.ToString();
                //Update Approval Status
                _dbContext.Update(objRequest);
                return response;
            }
            return response;
        }

        public Response PreApproveRequest(int requestId)
        {
            objRequest = _dbSet.Find(requestId);
            if (objRequest == null)
            {
                response.IsError = true;
                response.Message = MessageCode.E003.ToString();
            }
            else
            {
                response.DataModel = objRequest;
            }
            if (!response.IsError)
            {
                if (ApprovalStatus == enmApprovalStatus.A)
                {
                    if (objRequest.ApprovalStatus == "A")
                    {
                        response.IsError = true;
                        response.Message = MessageCode.E004.ToString();
                    }
                }
                if (ApprovalStatus == enmApprovalStatus.R)
                {
                    if (objRequest.ApprovalStatus == "R")
                    {
                        response.IsError = true;
                        response.Message = MessageCode.E005.ToString();
                    }
                }
            }
            return response;
        }

        public override void Presave(Request entity)
        {
            base.Presave(entity);
            entity.Person = JsonConvert.SerializeObject(entity.Person);
        }
    }
}
