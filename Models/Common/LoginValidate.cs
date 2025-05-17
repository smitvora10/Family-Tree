using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Models.Common;

[NotMapped]
public class LoginValidate
{
    public string Username { get; set; }
    public int UserId { get; set; }
    public int UserRoleId { get; set; }
    public string Token {  get; set; }
}
