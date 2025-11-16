using System.ComponentModel.DataAnnotations;

namespace FamilyTree.Models.Common
{
    public class VerifyOtpRequest
    {
        [Required]
        [Phone]
        [StringLength(15)]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OtpCode { get; set; } = string.Empty;
    }
}
