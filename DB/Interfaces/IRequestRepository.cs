using System.Data;
using FamilyTree.Models.Master;

namespace FamilyTree.DB.Interfaces
{
    public interface IRequestRepository : IBaseRepository<Request>
    {
        DataTable GetDetailedRequests();
        DataTable GetDetailedRequestById(int id);
    }
}
