using Aduanas.Aci.Seguridad.Api.DTOs.Permis;

namespace Aduanas.Aci.Seguridad.Api.DTOs.Rol;

public class RolDTO
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public List<PermisoDTO> Permisos { get; set; }
}
