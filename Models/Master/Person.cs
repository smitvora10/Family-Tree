using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master;

/// <summary>
/// Person
/// </summary>
public class Person
{
    /// <summary>
    /// Person Id
    /// </summary>
    [Key]
    public int PersonId { get; set; }

    /// <summary>
    /// First Name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last Name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? LastName { get; set; }

    /// <summary>
    /// Father (Person) - Id
    /// </summary>
    [Required]
    public int FatherId { get; set; }

    /// <summary>
    /// Mother (Person) - Id
    /// </summary>
    [Required]
    public int MotherId { get; set; }

    /// <summary>
    /// Spouse (Person) - Id
    /// </summary>
    [Required]
    public int SpouseId { get; set; }

    /// <summary>
    /// Birth Date
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Phone No.
    /// </summary>
    [StringLength(15, MinimumLength = 10)]
    public string? PhoneNo { get; set; }

    /// <summary>
    /// Gender
    /// </summary>
    [Required]
    [StringLength(1)]
    [Column(TypeName = "char(1)")] // Specifies the database column type as char(1)
    public string? Gender { get; set; } = "M";

    /// <summary>
    /// Date of Death
    /// </summary>
    public DateTime? DateOfDeath { get; set; } = null;

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Marital Status
    /// </summary>
    [Required]
    [StringLength(1)]
    [Column(TypeName = "char(1)")] // Specifies the database column type as char(1)
    public string? MaritalStatus { get; set; } = "N";

    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Occupation
    /// </summary>
    [DefaultValue(0)]
    public int? OccupationId { get; set; }

    /// <summary>
    /// Qualification
    /// </summary>
    [DefaultValue(0)]
    public int? QualificationId { get; set; }

    /// <summary>
    /// Creation Datetime
    /// </summary>
    public DateTime? CreationDatetime { get; set; } = null;

    /// <summary>
    /// Modified Datetime
    /// </summary>
    public DateTime? ModificationDatetime { get; set; } = null;

    /// <summary>
    /// Children Prop not to be added in DB
    /// </summary>
    [NotMapped]
    public List<Person> Children { get; set; } = new List<Person>();

    /// <summary>
    /// Mother Prop not to be added in DB
    /// </summary>
    [NotMapped]
    public Person Mother { get; set; } = null;

    /// <summary>
    /// Father Prop not to be added in DB
    /// </summary>
    [NotMapped]
    public Person Father { get; set; } = null;

    /// <summary>
    /// Father Prop not to be added in DB
    /// </summary>
    [NotMapped]
    public Person? Spouse { get; set; } = null;
}
