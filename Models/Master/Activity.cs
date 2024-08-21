using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// User Role
/// </summary>
public class Activity
{
    [Key]
    public int ActivityId { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "VARCHAR(100)")]
    public string ActivityName { get; set; }

    /// <summary>
    /// Modification Datetime
    /// </summary>
    [NotMapped]
    [Column(TypeName = "DATETIME")]
    public DateTime? ModificationDatetime { get; set; }
}
