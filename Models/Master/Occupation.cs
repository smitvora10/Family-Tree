using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// Person
/// </summary>
public class Occupation
{
    /// <summary>
    /// Occupation Id
    /// </summary>
    [Key]
    public int OccupationId { get; set; }

    /// <summary>
    /// Short Desc
    /// </summary>
    [Required]
    [StringLength(5)]
    public string? ShortDesc { get; set; }

    /// <summary>
    /// Occupation Name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>
    /// Occupation Description
    /// </summary>
    [Required]
    [StringLength(200)]
    public string? OccupationDesc { get; set; }


    /// <summary>
    /// Creation Datetime
    /// </summary>
    [NotMapped]
    public DateTime? CreationDatetime { get; set; } = null;

    /// <summary>
    /// Modified Datetime
    /// </summary>
    [NotMapped]
    public DateTime? ModificationDatetime { get; set; } = null;

}
