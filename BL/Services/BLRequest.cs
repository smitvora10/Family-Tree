using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Request = FamilyTree.Models.Master.Request;

namespace FamilyTree.BL.Services
{
    public class BLRequest : BLCommon<Request>, IRequestService
    {
        private readonly IRequestRepository _dbContext;
        private readonly DbSet<Request> _dbSet;
        private readonly IPersonService _personService;

        public Request objRequest { get; set; }
        public enmApprovalStatus ApprovalStatus { get; set; }

        public BLRequest(IRequestRepository dbContext, IPersonService personService, DataContext context) : base(dbContext)
        {
            _personService = personService;
            _dbContext = dbContext;
            _dbSet = context.Set<Request>();
        }

        public int CurrentUserId { get; set; }
        public int CurrentUserRole { get; set; }

        public override Response GetAll(string[]? includeFields = null, string[]? excludeFields = null)
        {
            int? userIdToFilter = null;
            if (CurrentUserRole == 2) // Member
            {
                userIdToFilter = CurrentUserId;
            }

            response = new Response
            {
                Data = _dbContext.GetDetailedRequests(userIdToFilter)
            };

            return response;
        }

        public override Response GetById(int id)
        {
            response = new Response();

            if (id == 0)
            {
                response.IsError = true;
                response.MessageCode = MessageCode.E001.ToString();
                return response;
            }

            var result = _dbContext.GetDetailedRequestById(id);

            if (result == null || result.Rows.Count == 0)
            {
                response.IsError = true;
                response.MessageCode = MessageCode.E001.ToString();
                return response;
            }

            // Check authorization for Member
            if (CurrentUserRole == 2)
            {
                // Assuming LastUpdatedUserId is in the result (we added it)
                if (result.Columns.Contains("LastUpdatedUserId"))
                {
                    var row = result.Rows[0];
                    if (row["LastUpdatedUserId"] != DBNull.Value)
                    {
                        int ownerId = Convert.ToInt32(row["LastUpdatedUserId"]);
                        if (ownerId != CurrentUserId)
                        {
                            response.IsError = true;
                            response.Message = "Unauthorized to view this request.";
                            return response;
                        }
                    }
                }
            }

            response.Data = result;
            return response;
        }

        public Response ApproveRequest()
        {
            if (objRequest == null || string.IsNullOrEmpty(objRequest.Person))
            {
                response.IsError = true;
                response.Message = "Invalid Request Data";
                return response;
            }

            Person? objPerson = JsonConvert.DeserializeObject<Person>(objRequest.Person);
            if (objPerson == null)
            {
                response.IsError = true;
                response.Message = "Invalid Person Data";
                return response;
            }

            _personService.EntryType = objRequest.Action;
            response = _personService.ValidationBeforePreSave(objPerson);
            if (!response.IsError)
            {
                _personService.Presave(objPerson);

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
            objRequest = _dbSet.Find(requestId)!;
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
                    if (objRequest!.ApprovalStatus == "A")
                    {
                        response.IsError = true;
                        response.Message = MessageCode.E004.ToString();
                    }
                }
                if (ApprovalStatus == enmApprovalStatus.R)
                {
                    if (objRequest!.ApprovalStatus == "R")
                    {
                        response.IsError = true;
                        response.Message = MessageCode.E005.ToString();
                    }
                }
            }
            return response;
        }

        public override Response Delete(int id)
        {
            if (CurrentUserRole == 2)
            {
                var req = _dbSet.Find(id);
                if (req != null && req.LastUpdatedUserId != CurrentUserId)
                {
                    response.IsError = true;
                    response.Message = "Unauthorized to delete this request.";
                    return response;
                }
            }
            return base.Delete(id);
        }

        public void Presave(DTORequest entity)
        {
            objRequest = new Request();
            objRequest.Action = entity.Action;
            objRequest.Person = JsonConvert.SerializeObject(entity.Person);
            objRequest.LastUpdatedUserId = CurrentUserId;
            base.Presave(objRequest);
        }

        public Response ValidationBeforePreSave(DTORequest entity)
        {
            return response;
        }

        public bool HasDuplicate(string tableName, DTORequest entity, params string[] keyFields)
        {
            throw new NotImplementedException();
        }
    }
}
