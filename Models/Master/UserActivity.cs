using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// User Role
/// </summary>
public class UserActivity
{
    [Key]
    public int UserActivityId { get; set; }

    [Required] 
    public int UserId { get; set; }

    public string ActivityIdsToAdd { get; set; } // Comma-separated activity IDs to add
    public string ActivityIdsToRemove { get; set; } // Comma-separated activity IDs to remove

    /// <summary>
    /// Modification Datetime
    /// </summary>
    [Column(TypeName = "DATETIME")]
    public DateTime? ModificationDatetime { get; set; }
}
