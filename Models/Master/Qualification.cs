using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// Person
/// </summary>
public class Qualification
{
    /// <summary>
    /// Qaulification Id
    /// </summary>
    [Key]
    public int QaulificationId { get; set; }

    /// <summary>
    /// Short Desc
    /// </summary>
    [Required]
    [StringLength(10)]
    public string? ShortDesc { get; set; }

    /// <summary>
    /// Qaulification Name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>
    /// Qaulification Description
    /// </summary>
    [Required]
    [StringLength(200)]
    public string? QaulificationDesc { get; set; }


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
