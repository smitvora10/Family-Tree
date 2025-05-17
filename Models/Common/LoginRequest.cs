using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Common;

[NotMapped]
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int UserRoleId { get; set; }
}
