using Aduanas.Aci.Seguridad.Api.DTOs.Auth;
using Aduanas.Aci.Seguridad.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aduanas.Aci.Seguridad.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Autenticar usuario y obtener tokens JWT</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ip     = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _authService.LoginAsync(request, ip);

        if (result is null)
            return Unauthorized(new { mensaje = "Credenciales inválidas o cuenta bloqueada" });

        return Ok(result);
    }

    /// <summary>Renovar AccessToken usando un RefreshToken válido</summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RefreshTokenAsync(request);

        if (result is null)
            return Unauthorized(new { mensaje = "Token inválido o expirado" });

        return Ok(result);
    }

    /// <summary>Cerrar sesión revocando el RefreshToken</summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        await _authService.LogoutAsync(refreshToken);
        return Ok(new { mensaje = "Sesión cerrada correctamente" });
    }

    /// <summary>Obtener información del usuario autenticado desde el token</summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMe()
    {
        var idUsuario    = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var usuarioLogin = User.Identity?.Name;
        var roles        = User.FindAll(System.Security.Claims.ClaimTypes.Role)
                               .Select(c => c.Value).ToList();
        var permisos     = User.FindAll("permiso")
                               .Select(c => c.Value).ToList();

        return Ok(new { idUsuario, usuarioLogin, roles, permisos });
    }
}
