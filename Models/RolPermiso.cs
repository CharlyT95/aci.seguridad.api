using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aduanas.Aci.Seguridad.Api.Models;

public class RolPermiso
{
    [Key]
    public int IdRolPermiso { get; set; }

    public int IdRol { get; set; }

    [ForeignKey("IdRol")]
    public virtual Rol? Rol { get; set; }

    public int IdPermiso { get; set; }

    [ForeignKey("IdPermiso")]
    public virtual Permiso? Permiso { get; set; }

    public bool Activo { get; set; }
}
