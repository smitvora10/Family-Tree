using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Common;

[NotMapped]
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int UserRoleId { get; set; }
}
