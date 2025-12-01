using FamilyTree.Core;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public interface IRequestService : IBaseService<DTORequest>
    {
        public enmApprovalStatus ApprovalStatus { get; set; }
        public int CurrentUserId { get; set; }
        public int CurrentUserRole { get; set; }
        Response PreApproveRequest(int requestId);
        Response ApproveRequest();
    }
}
