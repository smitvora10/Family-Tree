using FamilyTree.Core;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public interface IRequestService : IBaseService<Request>
    {
        public enmApprovalStatus ApprovalStatus { get; set; }
        Response PreApproveRequest(int requestiId);
        Response ApproveRequest();
    }
}
