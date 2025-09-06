using FamilyTree.BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class CustomAuthorizeFilter : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;
    private readonly ITokenService _tokenService;

    public CustomAuthorizeFilter(ITokenService tokenService, params string[] roles)
    {
        _roles = roles;
        _tokenService = tokenService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            return;

        var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string token = authHeader.Substring("Bearer ".Length).Trim();
        try
        {
            var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
            var (userId, roleId) = tokenService.ValidateToken(token);
            context.HttpContext.Items["UserId"] = userId;
            context.HttpContext.Items["RoleId"] = roleId;

            string roleName = roleId switch
            {
                1 => "Admin",
                2 => "Member",
                _ => throw new UnauthorizedAccessException("Invalid role")
            };

            bool isAuthorized = _roles.Length == 0 ? roleName == "Admin" : _roles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
            if (!isAuthorized)
                context.Result = new ForbidResult();
        }
        catch
        {
            context.Result = new UnauthorizedResult();
        }
    }
}

