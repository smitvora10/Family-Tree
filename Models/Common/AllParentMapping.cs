using System.ComponentModel.DataAnnotations;

namespace FamilyTree.Models.Master;

/// <summary>
/// Parent - Child Relation table(Child of Relation table)
/// </summary>
public class AllParentMapping
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
    /// Person 1 Id
    /// </summary>
    [Required]
    public int? Person1Id { get; set; }

    /// <summary>
    /// Person 2 Id
    /// </summary>
    [Required]
    public int? Person2Id { get; set; }
}
