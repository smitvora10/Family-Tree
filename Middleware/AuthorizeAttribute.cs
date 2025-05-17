using Microsoft.AspNetCore.Mvc;

public class AuthorizeAttribute : TypeFilterAttribute
{
    public AuthorizeAttribute(params string[] roles)
        : base(typeof(CustomAuthorizeFilter))
    {
        Arguments = new object[] { roles };
    }
}
