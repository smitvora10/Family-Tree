using Microsoft.AspNetCore.Mvc;

public class AuthorizeAttribute : TypeFilterAttribute
{
    public AuthorizeAttribute(params string[] roles)
        : this(false, roles)
    {
    }

    public AuthorizeAttribute(bool isGlobal, params string[] roles)
        : base(typeof(CustomAuthorizeFilter))
    {
        Arguments = new object[] { isGlobal, roles };
    }
}
