using Aduanas.Aci.Seguridad.Api.DTOs.Auth;
using Aduanas.Aci.Seguridad.Api.Helpers;
using Aduanas.Aci.Seguridad.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _authService.LoginAsync(request, ip);

        if (result is null)
            return Unauthorized(ResponseHelper.Fail<string>("Credenciales incorrectas, intente nuevamente"));

        return Ok(ResponseHelper.Success(result));
    }

    /// <summary>
    /// Renovar AccessToken usando un RefreshToken válido.
    /// Headers requeridos:
    ///   Authorization: Bearer {accessToken}
    ///   X-Refresh-Token: {refreshToken}
    /// </summary>
    [HttpGet("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        var accessToken = Request.Headers["Authorization"]
            .FirstOrDefault()?.Replace("Bearer ", "").Trim();

        var refreshToken = Request.Headers["X-Refresh-Token"]
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            return BadRequest(ResponseHelper.Fail<string>("Se requieren los headers Authorization y X-Refresh-Token."));

        var request = new RefreshTokenRequestDTO
        {
            Token = accessToken,
            TokenActualizacion = refreshToken
        };

        var result = await _authService.RefreshTokenAsync(request);

        if (result is null)
            return Unauthorized(ResponseHelper.Fail<string>("Token inválido o expirado"));

        return Ok(ResponseHelper.Success(result));
    }

    /// <summary>
    /// Cerrar sesión revocando el RefreshToken.
    /// Headers requeridos:
    ///   X-Refresh-Token: {refreshToken}
    /// </summary>
    [HttpGet("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Headers["X-Refresh-Token"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(refreshToken))
            return BadRequest(ResponseHelper.Fail<string>("El refresh token es requerido."));

        var resultado = await _authService.LogoutAsync(refreshToken);

        return resultado switch
        {
            SesionEnum.Revocado => Ok(ResponseHelper.Success<object>(null, "Sesión cerrada correctamente.")),
            SesionEnum.YaRevocado => Ok(ResponseHelper.Success<object>(null, "Sesión ya fue cerrada anteriormente.")),
            SesionEnum.NoEncontrado => NotFound(ResponseHelper.Fail<string>("Token no encontrado.")),
            _ => StatusCode(500, ResponseHelper.Fail<string>("Error inesperado."))
        };
    }

    /// <summary>Obtener información del usuario autenticado desde el token</summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var accessToken = Request.Headers["Authorization"]
            .FirstOrDefault()?.Replace("Bearer ", "").Trim();

        if (string.IsNullOrWhiteSpace(accessToken))
            return Unauthorized(ResponseHelper.Fail<string>("Token no proporcionado."));

        var valido = await _authService.IsAccessTokenActivoAsync(accessToken);
        if (!valido)
            return Unauthorized(ResponseHelper.Fail<string>("La sesión ha sido revocada."));

        var me = new MeDTO
        {
            idUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            usuarioLogin = User.Identity?.Name,
            roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
            permisos = User.FindAll("permiso").Select(c => c.Value).ToList()
        };

        return Ok(ResponseHelper.Success(me));
    }
}