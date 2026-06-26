using Aduanas.Aci.Seguridad.Api.Data;
using Aduanas.Aci.Seguridad.Api.Helpers;
using Aduanas.Aci.Seguridad.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Seguridad.Api.Services;

public interface ITokenService
{
    Task<RefreshToken> SaveRefreshTokenAsync(
        int idUsuario, string refreshToken, string accessToken, DateTime expiration, string? ip);
    Task<RefreshToken?> GetValidRefreshTokenAsync(string token);
    Task<SesionEnum> RevokeRefreshTokenAsync(string refreshToken);
    Task<bool> IsAccessTokenActivoAsync(string accessToken);
}

public class TokenService : ITokenService
{
    private readonly AppDbContext _db;

    public TokenService(AppDbContext db) => _db = db;

    public async Task<RefreshToken> SaveRefreshTokenAsync(
        int idUsuario, string refreshToken, string accessToken, DateTime expiration, string? ip)
    {
        var entity = new RefreshToken
        {
            IdUsuario = idUsuario,
            TokenHash = accessToken,       // access token (texto plano)
            TokenHashReemplazo = refreshToken,      // refresh token (texto plano)
            Expira = expiration,
            FechaCreado = DateTime.Now,
            Revocado = false,
            FechaRevocado = null
        };

        _db.RefreshToken.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token)
    {
        return await _db.RefreshToken
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>
                r.TokenHashReemplazo == token &&
                !r.Revocado &&
                r.Expira > DateTime.Now);
    }

    /// <summary>
    /// Revoca la sesión buscando por RefreshToken (TokenHashReemplazo).
    /// </summary>
    public async Task<SesionEnum> RevokeRefreshTokenAsync(string refreshToken)
    {
        var rt = await _db.RefreshToken
            .FirstOrDefaultAsync(r => r.TokenHashReemplazo == refreshToken);

        if (rt is null)
            return SesionEnum.NoEncontrado;

        if (rt.Revocado)
            return SesionEnum.YaRevocado;

        rt.Revocado = true;
        rt.FechaRevocado = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return SesionEnum.Revocado;
    }

    /// <summary>
    /// Verifica si el access token tiene una sesión activa (no revocada) en BD.
    /// Compara contra TokenHash, que se guarda en texto plano al hacer login.
    /// </summary>
    public async Task<bool> IsAccessTokenActivoAsync(string accessToken)
    {
        var sesion = await _db.RefreshToken
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.TokenHash == accessToken);

        if (sesion is null)
            return false;

        return !sesion.Revocado;
    }
}