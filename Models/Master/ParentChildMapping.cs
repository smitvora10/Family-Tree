using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// Parent - Child Relation table(Child of Relation table)
/// </summary>
public class ParentChildMapping
{
    /// <summary>
    /// Parent - Child Mapping Id
    /// </summary>
    [Key]
    public int ParentChildMappingId { get; set; }

    /// <summary>
    /// Relation Type Id
    /// </summary>
    [Required]
    public int? RelationTypeId { get; set; }

    /// <summary>
    /// Parent Id
    /// </summary>
    [Required]
    public int? ParentId { get; set; }

    /// <summary>
    /// Child Id
    /// </summary>
    [Required]
    public int? ChildId { get; set; }
}
