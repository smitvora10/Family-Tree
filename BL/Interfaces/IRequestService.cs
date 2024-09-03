using FamilyTree.Models.Common;
using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public interface IRequestService : IBaseService<Request>
    {
        Response PreApproveRequest(int requestiId);
        Response ApproveRequest();
    }
}
