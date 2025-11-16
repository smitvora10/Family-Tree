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

    [Required]
    [StringLength(15)]
    [Column(TypeName = "varchar(15)")]
    public string MobileNumber { get; set; } = string.Empty;


    /// <summary>
    /// Password
    /// </summary>
    [Required]
    [StringLength(25)]
    [NotMapped]
    public string? Password { get; set; }

    /// <summary>
    /// PasswordHash
    /// </summary>
    [StringLength(250)]
    [Column(TypeName = "varchar(250)")]
    public string? PasswordHash { get; set; }

    /// <summary>
    /// User Role Id
    /// </summary>
    [Required]
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
