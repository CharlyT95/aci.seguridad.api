using Aduanas.Aci.Seguridad.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Seguridad.Api.Services;

public interface IPermissionService
{
    Task<bool> TienePermisoAsync(int idUsuario, string codigoPermiso);
    Task<List<string>> ObtenerPermisosAsync(int idUsuario);
}

public class PermisoService : IPermissionService
{
    private readonly AppDbContext _db;

    public PermisoService(AppDbContext db) => _db = db;

    public async Task<List<string>> ObtenerPermisosAsync(int idUsuario)
    {
        var idRoles = await _db.UsuarioRol
            .AsNoTracking()
            .Where(ur => ur.IdUsuario == idUsuario)
            .Select(ur => ur.IdRol)
            .ToListAsync();

        return await _db.RolPermiso
            .AsNoTracking()
            .Where(rp => idRoles.Contains(rp.IdRol))
            .Select(rp => rp.Permiso!.CodigoPermiso)
            .Distinct()
            .ToListAsync();
    }

    public async Task<bool> TienePermisoAsync(int idUsuario, string codigoPermiso)
    {
        var idRoles = await _db.UsuarioRol
            .AsNoTracking()
            .Where(ur => ur.IdUsuario == idUsuario)
            .Select(ur => ur.IdRol)
            .ToListAsync();

        return await _db.RolPermiso
            .AsNoTracking()
            .AnyAsync(rp =>
                idRoles.Contains(rp.IdRol) &&
                rp.Permiso!.CodigoPermiso == codigoPermiso);
    }
}
