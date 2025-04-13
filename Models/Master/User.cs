using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// User Role
/// </summary>
public class User
{
    /// <summary>
    /// User Id
    /// </summary>
    [Key]
    public int UserId { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? Username { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? Password { get; set; }

    /// <summary>
    /// User Role Id
    /// </summary>
    public int UserRoleId { get; set; } = 0;

    /// <summary>
    /// last Updated User Id
    /// </summary>
    public int LastUpdatedUserId { get; set; } = 0;

    /// <summary>
    /// Creation Datetime
    /// </summary>
    [Column(TypeName = "DATETIME")]
    [DefaultValue("CURRENT_TIMESTAMP")]
    public DateTime? CreationDatetime { get; set; }

    /// <summary>
    /// Modification Datetime
    /// </summary>
    [Column(TypeName = "DATETIME")]
    [DefaultValue("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")]
    public DateTime? ModificationDatetime { get; set; }


}
