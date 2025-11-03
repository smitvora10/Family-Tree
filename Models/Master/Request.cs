using FamilyTree.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// Person Request
/// </summary>
public class Request
{
    /// <summary>
    /// Request Id
    /// </summary>
    [Key]
    public int RequestId { get; set; }

    /// <summary>
    /// Admin Approval Status (A = Approved, P = Pending, R = Rejected)
    /// </summary>
    [Required]
    [StringLength(1)]
    [Column(TypeName = "char(1)")] // Specifies the database column type as char(1)
    public string ApprovalStatus { get; set; } = "P";

    /// <summary>
    /// Person Json For Req.
    /// </summary>
    [Required]
    public string Person { get; set; }

    /// <summary>
    /// last Updated User Id
    /// </summary>
    public int LastUpdatedUserId { get; set; } = 0;

    /// <summary>
    /// Action A/E/D
    /// </summary>
    public enmEntryType Action = enmEntryType.A;

    /// <summary>
    /// Creation Datetime
    /// </summary>
    [Column(TypeName = "DATETIME")]
    public DateTime CreationDatetime { get; set; }

    /// <summary>
    /// Approved Datetime
    /// </summary>
    [Column(TypeName = "DATETIME")]
    public DateTime ApprovedDatetime { get; set; }
}

public class DTORequest
{
        /// <summary>
    /// Person Json For Req.
    /// </summary>
    [Required]
    public Person Person { get; set; }


    /// <summary>
    /// Action A/E/D
    /// </summary>
    public enmEntryType Action = enmEntryType.A;

}


