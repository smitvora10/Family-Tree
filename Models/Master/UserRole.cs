using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// User Role
/// </summary>
public class UserRole
{
    /// <summary>
    /// User Role Id
    /// </summary>
    [Key]
    public int UserRoleId { get; set; }

    /// <summary>
    /// User Role Description
    /// </summary>
    [Required]
    [StringLength(150)]
    [Column(TypeName = "varchar(150)")]
    public string? UserRoleDescription { get; set; }

    /// <summary>
    /// Role Activity Ids
    /// </summary>
    [Required]
    public string? RoleActivityIds { get; set; }

    /// <summary>
    /// Is Super User Role
    /// </summary>
    [Required]
    [StringLength(1)]
    [Column(TypeName = "char(1)")] // Specifies the database column type as char(1)
    public string IsSuperUserRole { get; set; } = "N";

    /// <summary>
    /// last Updated User Id
    /// </summary>
    public int LastUpdatedUserId { get; set; } = 0;

    /// <summary>
    /// Creation Datetime
    /// </summary>
    [Column(TypeName = "DATETIME")]
    public DateTime? CreationDatetime { get; set; }

    /// <summary>
    /// Modification Datetime
    /// </summary>
    [Column(TypeName = "DATETIME")]
    public DateTime? ModificationDatetime { get; set; }


}
