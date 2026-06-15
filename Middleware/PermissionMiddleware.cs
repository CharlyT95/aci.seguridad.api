using Aduanas.Aci.Seguridad.Api.Services;
using System.Security.Claims;

namespace Aduanas.Aci.Seguridad.Api.Middleware;

public class PermissionMiddleware
{
    private readonly RequestDelegate _next;

    public PermissionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IPermissionService permissionService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int idUsuario))
            {
                // Cargar permisos actualizados y exponerlos en el contexto HTTP
                var permisos = await permissionService.ObtenerPermisosAsync(idUsuario);
                context.Items["UserPermissions"] = permisos;
            }
        }

        await _next(context);
    }
}
