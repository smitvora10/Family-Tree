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
    private readonly bool _isGlobal;

    public CustomAuthorizeFilter(ITokenService tokenService, bool isGlobal, string[] roles)
    {
        _roles = roles;
        _tokenService = tokenService;
        _isGlobal = isGlobal;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            return;

        // If this is the global filter and the action has a specific AuthorizeAttribute, skip this global check.
        // The action-level filter will handle the authorization.
        if (_isGlobal && context.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any())
        {
            return;
        }

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

            //If no role given in APi then considered for admin role only if rolename given then checked for it
            bool isAuthorized = _roles.Length == 0 || roleName == "Admin" || _roles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
            if (!isAuthorized)
                context.Result = new UnauthorizedResult();
        }
        catch
        {
            context.Result = new UnauthorizedResult();
        }
    }
}

