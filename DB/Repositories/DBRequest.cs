using System.Data;
using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public class DBRequest : DBCommon<Request>, IRequestRepository
    {
        public DBRequest(DataContext context) : base(context)
        {
        }

        public DataTable GetDetailedRequests(int? userId = null)
        {
            string sql = @"
SELECT
    r.RequestId,
    r.ApprovalStatus AS ApprovalStatusSymbol,
    CASE r.ApprovalStatus
        WHEN 'A' THEN 'Approved'
        WHEN 'P' THEN 'Pending'
        WHEN 'R' THEN 'Rejected'
        WHEN 'E' THEN 'Error'
        ELSE 'Pending'
    END AS ApprovalStatusDescription,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.PersonId')) AS PersonId,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.FirstName')) AS PersonFirstName,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.LastName')) AS PersonLastName,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Gender')) AS PersonGender,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.FatherId')) AS PersonFatherId,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.MotherId')) AS PersonMotherId,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.BirthDate')) AS PersonBirthDate,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.PhoneNo')) AS PersonPhoneNo,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Description')) AS PersonDescription,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.MaritalStatus')) AS PersonMaritalStatus,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Address')) AS PersonAddress,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.OfficeAddress')) AS PersonOfficeAddress,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Occupation')) AS PersonOccupation,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Qualification')) AS PersonQualification,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.IsMainPerson')) AS PersonIsMainPerson,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Town')) AS PersonTown,
    r.Person AS PersonJson,
    CASE r.Action
        WHEN 0 THEN 'A'
        WHEN 1 THEN 'E'
        WHEN 2 THEN 'D'
        ELSE ''
    END AS ActionSymbol,
    COALESCE(u.Username, '') AS LastUpdatedBy,
    r.LastUpdatedUserId,
    r.CreationDatetime,
    r.ApprovedDatetime
FROM Request r
LEFT JOIN `User` u ON r.LastUpdatedUserId = u.UserId";

            if (userId.HasValue && userId.Value > 0)
            {
                sql += " WHERE r.LastUpdatedUserId = @p0";
                return ExecuteSql(sql, userId.Value);
            }

            return ExecuteSql(sql);
        }

        public DataTable GetDetailedRequestById(int id)
        {
            const string sql = @"
SELECT
    r.RequestId,
    r.ApprovalStatus AS ApprovalStatusSymbol,
    CASE r.ApprovalStatus
        WHEN 'A' THEN 'Approved'
        WHEN 'P' THEN 'Pending'
        WHEN 'R' THEN 'Rejected'
        WHEN 'E' THEN 'Error'
        ELSE 'Pending'
    END AS ApprovalStatusDescription,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.PersonId')) AS PersonId,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.FirstName')) AS PersonFirstName,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.LastName')) AS PersonLastName,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Gender')) AS PersonGender,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.FatherId')) AS PersonFatherId,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.MotherId')) AS PersonMotherId,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.BirthDate')) AS PersonBirthDate,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.PhoneNo')) AS PersonPhoneNo,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Description')) AS PersonDescription,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.MaritalStatus')) AS PersonMaritalStatus,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Address')) AS PersonAddress,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.OfficeAddress')) AS PersonOfficeAddress,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Occupation')) AS PersonOccupation,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Qualification')) AS PersonQualification,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.IsMainPerson')) AS PersonIsMainPerson,
    JSON_UNQUOTE(JSON_EXTRACT(r.Person, '$.Town')) AS PersonTown,
    r.Person AS PersonJson,
    CASE r.Action
        WHEN 0 THEN 'A'
        WHEN 1 THEN 'E'
        WHEN 2 THEN 'D'
        ELSE ''
    END AS ActionSymbol,
    COALESCE(u.Username, '') AS LastUpdatedBy,
    r.LastUpdatedUserId,
    r.CreationDatetime,
    r.ApprovedDatetime
FROM Request r
LEFT JOIN `User` u ON r.LastUpdatedUserId = u.UserId
WHERE r.RequestId = @p0
LIMIT 1";

            return ExecuteSql(sql, id);
        }
    }
}
