using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// Relation Type
/// </summary>
public class RelationType
{
    /// <summary>
    /// Relation Type Id
    /// </summary>
    [Key]
    public int RelationTypeId { get; set; }

    /// <summary>
    /// Relation Type Code
    /// </summary>
    [Required]
    [StringLength(2)]
    [Column(TypeName = "char(2)")] // Specifies the database column type as char(1)
    public string? RelationTypeCode { get; set; }

    /// <summary>
    /// Relation Type Description
    /// </summary>
    [Required]
    [StringLength(150)]
    [Column(TypeName = "varchar(150)")] // Specifies the database column type as char(1)
    public string? RelationTypeDescription { get; set; }

    /// <summary>
    /// last Updated User Id
    /// </summary>
    public int LastUpdatedUserId { get; set; } = 0;

    /// <summary>
    /// Creation Datetime
    /// </summary>
    [Required]
    [Column(TypeName = "DATETIME")]
    public DateTime CreationDatetime { get; set; }

    /// <summary>
    /// Modification Datetime
    /// </summary>
    [Required]
    [Column(TypeName = "DATETIME")]
    public DateTime ModificationDatetime { get; set; }
}
