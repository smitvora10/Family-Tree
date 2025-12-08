namespace FamilyTree.Models.Common
{
    public class UpdateProfileRequest
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
    }
}
