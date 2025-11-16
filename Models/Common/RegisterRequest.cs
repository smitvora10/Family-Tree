using System.ComponentModel.DataAnnotations;

namespace FamilyTree.Models.Common
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(15)]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        public int UserRoleId { get; set; } = 2;
    }
}
