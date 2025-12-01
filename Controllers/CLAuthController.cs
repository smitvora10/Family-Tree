using FamilyTree.BL.Services;
using System.Threading.Tasks;
using FamilyTree.Models.Common;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Auth")]
public class CLAuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public CLAuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        Response response = await _authService.RegisterUser(registerRequest);
        return Ok(response);
    }

    [HttpPost("verify-otp")]
    [AllowAnonymous]
    public IActionResult VerifyOtp([FromBody] VerifyOtpRequest verifyOtpRequest)
    {
        Response response = _authService.VerifyOtp(verifyOtpRequest);
        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        Response response = _authService.ValidateUser(loginRequest);
        return Ok(response);
    }
}
