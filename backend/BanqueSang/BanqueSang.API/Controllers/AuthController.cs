using BanqueSang.API.DTOs.Auth;
using BanqueSang.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Connecte un utilisateur à partir de son login et de son mot de passe.
    /// Retourne un token JWT si l'authentification réussit.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var result = await _authService.LoginAsync(request.Login, request.Password);

            var response = new LoginResponseDto
            {
                Token = result.Token,
                Role = result.Role,
                Expiration = result.Expiration
            };

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                message = ex.Message
            });
        }
    }
    
}

