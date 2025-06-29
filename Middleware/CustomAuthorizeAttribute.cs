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
        bool allowAnonymous = context.ActionDescriptor.EndpointMetadata
            .OfType<AllowAnonymousAttribute>().Any();

        if (allowAnonymous)
            return;

        HttpRequest request = context.HttpContext.Request;
        string? authHeader = request.Headers["Authorization"].FirstOrDefault();

        if (authHeader == null || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string token = authHeader.Substring("Bearer ".Length).Trim();

        int userId;
        int roleId;

        try
        {
            ITokenService tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
            (userId, roleId) = tokenService.ValidateToken(token);

            context.HttpContext.Items["UserId"] = userId;
            context.HttpContext.Items["RoleId"] = roleId;
        }
        catch
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        int userIdObj = Convert.ToInt32(context.HttpContext.Items["UserId"]);
        int roleIdObj = Convert.ToInt32(context.HttpContext.Items["RoleId"]);

        if (userIdObj != userId || roleIdObj != roleId)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string roleName = roleId switch
        {
            1 => "Admin",
            2 => "Member",
            _ => "Unknown"
        };

        if (_roles.Length == 0 || roleName == "Admin")
            return;

        if (!_roles.Contains(roleName, StringComparer.OrdinalIgnoreCase))
        {
            context.Result = new ForbidResult();
        }
    }
}

