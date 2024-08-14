using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// Relation Mapping
/// </summary>
public class RelationMapping
{
    [Key]
    public int RelationId { get; set; }

    /// <summary>
    /// Person1_Id
    /// </summary>

    [Required]
    public int? Person1Id { get; set; }

    /// <summary>
    /// Person2_Id
    /// </summary>
    [Required]
    public int? Person2Id { get; set; }

    /// <summary>
    /// Relation_Type_Id
    /// </summary>
    [Required]
    public int? RelationTypeId { get; set; }

    /// <summary>
    /// Relation_Type Code
    /// </summary>
    [Required]
    [StringLength(2)]
    [Column(TypeName = "char(2)")] // Specifies the database column type as char(2)
    public string? RelationTypeCode { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    [StringLength(100)]
    public string? Description { get; set; }

    /// <summary>
    /// last Updated User Id
    /// </summary>
    public int LastUpdatedUserId { get; set; } = 0;

    /// <summary>
    /// Creation Datetime
    /// </summary>
    public DateTime? CreationDatetime { get; set; }

    public DateTime? ModificationDatetime { get; set; }     

}
