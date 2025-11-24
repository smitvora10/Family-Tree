using System.ComponentModel.DataAnnotations;

namespace FamilyTree.Models.Common
{
    public class VerifyOtpRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OtpCode { get; set; } = string.Empty;
    }
}
