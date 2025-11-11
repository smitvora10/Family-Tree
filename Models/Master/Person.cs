using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    /// Middle Name
    /// </summary>
    //[Required]
    //[StringLength(100)]
    //public string? MiddleName { get; set; }

    /// <summary>
    /// Last Name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? LastName { get; set; }

    /// <summary>
    /// Father (Person) - Id
    /// </summary>
    public int FatherId { get; set; }

    /// <summary>
    /// Mother (Person) - Id
    /// </summary>
    public int MotherId { get; set; }

    /// <summary>
    /// Spouse (Person) - Id
    /// </summary>
    //[Required]
    //public int SpouseId { get; set; }

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
    //public DateTime? DateOfDeath { get; set; } = null;

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
    /// Address
    /// </summary>
    public string? OfficeAddress { get; set; }

    /// <summary>
    /// Occupation
    /// </summary>
    //[DefaultValue(0)]
    public string? Occupation { get; set; }

    /// <summary>
    /// Qualification
    /// </summary>
    //[DefaultValue(0)]
    public string? Qualification { get; set; }

    /// <summary>
    /// Is Main Person
    /// </summary>
    [StringLength(1)]
    [Column(TypeName = "char(1)")] // Specifies the database column type as char(1)
    public string? IsMainPerson { get; set; } = "N";

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

    /// <summary>
    /// Children Prop not to be added in DB
    /// </summary>
    [NotMapped]
    public List<Person> Children { get; set; } = new List<Person>();

    ///// <summary>
    ///// Mother Prop not to be added in DB
    ///// </summary>
    //[NotMapped]
    //[JsonIgnore]
    //public Person Mother { get; set; } = null;

    ///// <summary>
    ///// Father Prop not to be added in DB
    ///// </summary>
    //[NotMapped]
    //[JsonIgnore]
    //public Person Father { get; set; } = null;

    ///// <summary>
    ///// Father Prop not to be added in DB
    ///// </summary>
    //[NotMapped]
    //[JsonIgnore]
    //public Person? Spouse { get; set; } = null;

    ///// <summary>
    ///// Image Blob field
    ///// </summary>
    //[JsonIgnore]
    //public byte[] Image { get; set; } = null;  // This is the BLOB field

    /// <summary>
    /// Person image stored as binary.
    /// </summary>
    [Column(TypeName = "LONGBLOB")]
    [JsonIgnore]
    public byte[]? PersonImage { get; set; }

    /// <summary>
    /// Person image represented as a Base64 string for transport.
    /// </summary>
    [NotMapped]
    public string? PersonImageBase64 { get; set; }

    /// <summary>
    /// Town (Mur gaam)
    /// </summary>
    public string Town = null;
}
