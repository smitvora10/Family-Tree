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

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        Response response = await _authService.RegisterUser(registerRequest);
        return Ok(response);
    }

    [HttpPost("VerifyOtp")]
    [AllowAnonymous]
    public IActionResult VerifyOtp([FromBody] VerifyOtpRequest verifyOtpRequest)
    {
        Response response = _authService.VerifyOtp(verifyOtpRequest);
        return Ok(response);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        Response response = _authService.ValidateUser(loginRequest);
        return Ok(response);
    }

    [HttpPost("UpdateProfile")]
    [Authorize]
    public IActionResult UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        Response response = _authService.UpdateProfile(request);
        return Ok(response);
    }

    [HttpPost("SendOtp")]
    [AllowAnonymous]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
    {
        Response response = await _authService.SendOtp(request);
        return Ok(response);
    }

    [HttpPost("ChangePassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        Response response = await _authService.ChangePassword(request);
        return Ok(response);
    }
}
