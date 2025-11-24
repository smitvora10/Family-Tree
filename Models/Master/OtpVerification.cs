using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Master
{
    public class OtpVerification
    {
        [Key]
        public int OtpVerificationId { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "varchar(150)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6)]
        [Column(TypeName = "varchar(6)")]
        public string OtpCode { get; set; } = string.Empty;

        [Column(TypeName = "DATETIME")]
        public DateTime ExpirationTime { get; set; }

        public bool IsUsed { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? UsedAt { get; set; }
    }
}
