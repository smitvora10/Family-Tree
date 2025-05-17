using FamilyTree.BL.Services;
using FamilyTree.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CLAuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IAuthService _authService;

    public CLAuthController(ITokenService tokenService, IAuthService authService)
    {
        _tokenService = tokenService;
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        // Example: Validate from database
        Response response = _authService.ValidateUser(loginRequest);

        if (!response.IsError)
        {
            LoginValidate objLV = new();
            objLV.Username = loginRequest.Username;
            objLV.UserRoleId = loginRequest.UserRoleId;
            objLV.UserId = (int)response.Id;
            objLV.Token = _tokenService.GenerateToken((int)response.Id, loginRequest.UserRoleId);


            response.DataModel = objLV;
        }
        return Ok(response);
    }


}


