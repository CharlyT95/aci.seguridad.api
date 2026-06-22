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
    Task<SesionEnum> RevokeRefreshTokenAsync(string token);
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
            TokenHash = accessToken,
            TokenHashReemplazo = refreshToken,   
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


    //public async Task RevokeRefreshTokenAsync(string token)
    //{
    //    var rt = await _db.RefreshToken
    //        .FirstOrDefaultAsync(r => r.TokenHash == token && !r.Revocado);
    //    if (rt is not null)
    //    {
    //        rt.Revocado = true;
    //        rt.FechaRevocado = DateTime.UtcNow;
    //        await _db.SaveChangesAsync();
    //    }
    //}

    public async Task<SesionEnum> RevokeRefreshTokenAsync(string token)
    {
        var rt = await _db.RefreshToken
            .FirstOrDefaultAsync(r => r.TokenHash == token);

        if (rt is null)
            return SesionEnum.NoEncontrado;

        if (rt.Revocado)
            return SesionEnum.YaRevocado;

        rt.Revocado = true;
        rt.FechaRevocado = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return SesionEnum.Revocado;
    }
}